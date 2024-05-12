using IZO.Models.Earnings;
using IZO.Models.Expenses;
using IZO.Services;
using Microsoft.AspNetCore.Mvc;

namespace IZO.Controllers
{
    public class EarningController : Controller
    {
        private readonly ILogger<EarningController> _logger;

        public EarningController(ILogger<EarningController> logger)
        {
            _logger = logger;
        }

        public IActionResult GeneratePlan(double value)
        {
            // The add expense logic now resides here...

            var currentDate = DateTime.Now;
            var lastDayOfPreviousMonth = new DateTime(currentDate.Year, currentDate.Month, 1).AddDays(-1);

            double expenses = 0;
            double earnings = 0;

            foreach (var category in ExpenseAccesorService.monthlyExpenses.dayToDayExpenses)
            {
                foreach (var expense in category.Value.Where(x => x.Date < lastDayOfPreviousMonth))
                {
                    expenses = +expense.moneySpent;
                }
            }

            foreach (var category in EarningAccesorService.monthlyEarnings.fixed1Earnings)
            {
                foreach (var earning in category.Value.Where(x => x.Date < lastDayOfPreviousMonth))
                {
                    earnings = +earning.moneyEarn;
                }
            }



            int months = (int)(value / (earnings - expenses));
            if (months < 0) { return Json(new { success = true, planedSaving = 99999 }); }

            return Json(new { success = true, planedSaving = months });


        }
       
        public IActionResult AddEarning(string type, DateTime date, string category, double value)
        {
            // The add expense logic now resides here...
            if (Enum.TryParse(category, out EarningCategory expenseCategory))
            {
                Earning newEarning = new Earning
                {
                    Date = date,
                    category = expenseCategory,
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
                            break;
                        }
                        EarningAccesorService.monthlyEarnings.fixed1Earnings.Add(newEarning.category, new Earning[] { newEarning });

                        break;
                  
                }
                EarningAccesorService.saveExpenses();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }

        public IActionResult DeleteEarning(string type, DateTime date, string category, double value)
        {
            // The add expense logic now resides here...
            if (Enum.TryParse(category, out EarningCategory earningCategory))
            {

                switch (type)
                {
                    case "FIXED1":
                        if (EarningAccesorService.monthlyEarnings.fixed1Earnings.ContainsKey(earningCategory))
                        {
                            List<Earning> earnings = EarningAccesorService.monthlyEarnings.fixed1Earnings[earningCategory].ToList();
                            earnings.Remove(earnings.Single(earning => earning.category == earningCategory && earning.moneyEarn == value && earning.Date == date));
                            EarningAccesorService.monthlyEarnings.fixed1Earnings[earningCategory] = earnings.ToArray();
                        }
                        break;
                 
                }
                ExpenseAccesorService.saveExpenses();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }
    }
}
