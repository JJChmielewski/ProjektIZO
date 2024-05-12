using IZO.Models.Earnings;
using IZO.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IZO.Controllers
{
    public class EarningController : Controller
    {
        private readonly ILogger<EarningController> _logger;

        public EarningController(ILogger<EarningController> logger)
        {
            _logger = logger;
        }

        public IActionResult AddEarning(string type, DateTime date, string category, double value)
        {
            if (Enum.TryParse(category, out EarningCategory earningCategory))
            {
                Earning newEarning = new Earning
                {
                    Date = date,
                    category = earningCategory,
                    moneyEarn = value
                };

                _logger.LogInformation($"New earning added: {newEarning}");

                switch (type)
                {
                    case "FIXED1":
                        if (EarningAccesorService.monthlyEarnings.fixed1Earnings.ContainsKey(newEarning.category))
                        {
                            List<Earning> earnings = EarningAccesorService.monthlyEarnings.fixed1Earnings[newEarning.category].ToList();
                            earnings.Add(newEarning);
                            EarningAccesorService.monthlyEarnings.fixed1Earnings[newEarning.category] = earnings.ToArray();
                        }
                        else
                        {
                            EarningAccesorService.monthlyEarnings.fixed1Earnings.Add(newEarning.category, new Earning[] { newEarning });
                        }
                        break;
                }

                EarningAccesorService.saveEarnings(); // Corrected from saveExpenses to saveEarnings

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }

        public IActionResult DeleteEarning(string type, DateTime date, string category, double value)
        {
            if (Enum.TryParse(category, out EarningCategory earningCategory))
            {
                switch (type)
                {
                    case "FIXED1":
                        if (EarningAccesorService.monthlyEarnings.fixed1Earnings.ContainsKey(earningCategory))
                        {
                            List<Earning> earnings = EarningAccesorService.monthlyEarnings.fixed1Earnings[earningCategory].ToList();
                            Earning toRemove = earnings.SingleOrDefault(e => e.Date == date && e.category == earningCategory && e.moneyEarn == value);
                            if (toRemove != null)
                            {
                                earnings.Remove(toRemove);
                                EarningAccesorService.monthlyEarnings.fixed1Earnings[earningCategory] = earnings.ToArray();
                            }
                        }
                        break;
                }

                EarningAccesorService.saveEarnings(); // Corrected from saveExpenses to saveEarnings

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }

        // New method to fetch earnings data for visualization
        public IActionResult GetMonthlyEarningsSummary()
        {
            var earningsData = EarningAccesorService.monthlyEarnings.fixed1Earnings
                .SelectMany(kv => kv.Value)
                .GroupBy(e => new { Year = e.Date.Year, Month = e.Date.Month })
                .Select(g => new {
                    Month = $"{g.Key.Month:00}/{g.Key.Year}",
                    TotalEarnings = g.Sum(e => e.moneyEarn)
                })
                .OrderBy(x => x.Month)
                .ToList();  // Ensure this is serialized as a list to make handling on the client-side easier

            return Json(earningsData);
        }
    }
}
