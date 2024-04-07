using Microsoft.AspNetCore.Mvc;
using IZO.Models.Charts;

namespace IZO.Controllers
{
    public class PieChartController : Controller
    {
        public IActionResult Index()
        {
            var pieChartData = ExpensePieChart.GetMockData(); // Get the mock data
            return View(pieChartData); // Pass it to the view
        }
    }
}
