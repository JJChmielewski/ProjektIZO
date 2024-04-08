using IZO.Models;
using IZO.Models.Charts;
using IZO.Models.Expenses;
using IZO.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
                MonthlyExpenses = MonthlyExpenses.GetMockData(),
                ExpensePieChart = ExpensePieChart.GetMockData()
            };

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
