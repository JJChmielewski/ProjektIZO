using IZO.Models;
using IZO.Models.Charts;
using IZO.Models.Expenses;
using IZO.Models.ViewModels;
using IZO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace IZO.Controllers
{
    //main controller
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Fetch the mock data for both MonthlyExpenses and ExpensePieChart
            var viewModel = new ExpensesViewModel
            {
                ExpensePieChart = ExpensePieChart.initChartData()
            };

            List<string> availableMonths = new List<string>();
            List<double> expensesPerMonth = new List<double>();
            List<double> earningsPerMonth = new List<double>();

            foreach (string file in Directory.GetFiles(ExpenseAccesorService.pathToSavedDir))
            {
                var temp = ExpenseAccesorService.loadExpenses(file);
                double sum = 0;
                string month = file.Replace(".json", "").Replace(ExpenseAccesorService.pathToSavedDir, "");
                if (!availableMonths.Contains(month))
                {
                    availableMonths.Add(month);
                }

                foreach (var expense in temp.fixedExpenses.Values)
                {
                    sum += -1 * expense.Sum(e => e.moneySpent);
                }

                foreach (var expense in temp.dayToDayExpenses.Values)
                {
                    sum += -1 * expense.Sum(e => e.moneySpent);
                }
                expensesPerMonth.Add(sum);
            }

            foreach (string file in Directory.GetFiles(EarningAccesorService.pathToSavedDir))
            {
                var temp = EarningAccesorService.loadEarnings(file);
                double sum = 0;
                string month = file.Replace(".json", "").Replace(EarningAccesorService.pathToSavedDir, "");
                if (!availableMonths.Contains(month))
                {
                    availableMonths.Add(month);
                }

                foreach (var expense in temp.dayToDayEarnings.Values)
                {
                    sum += expense.Sum(e => e.moneyEarn);
                }

                foreach (var expense in temp.fixed1Earnings.Values)
                {
                    sum += expense.Sum(e => e.moneyEarn);
                }
                earningsPerMonth.Add(sum);
            }

            ViewBag.availableMonths = JsonSerializer.Serialize(availableMonths);
            ViewBag.expensesPerMonth = JsonSerializer.Serialize(expensesPerMonth);
            ViewBag.earningsPerMonth = JsonSerializer.Serialize(earningsPerMonth);

            // Pass the ViewModel to the view
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }
    }
}
