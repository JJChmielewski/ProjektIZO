using Microsoft.AspNetCore.Mvc;
using IZO.Services; 
using System.Linq;  

namespace IZO.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialDataController : ControllerBase
    {
        [HttpGet("GetFinancialData")]
        public IActionResult GetFinancialData()
        {
            var expenses = ExpenseAccesorService.monthlyExpenses;

            var earningsData = expenses.earnings
                .SelectMany(e => e.Value)
                .GroupBy(e => e.Date.ToString("MMMM"))
                .Select(g => new { Month = g.Key, Total = g.Sum(i => i.moneySpent) });

            var expensesData = expenses.fixedExpenses
                .Concat(expenses.dayToDayExpenses)
                .SelectMany(e => e.Value)
                .GroupBy(e => e.Date.ToString("MMMM"))
                .Select(g => new { Month = g.Key, Total = g.Sum(i => i.moneySpent) });

            return Ok(new { Earnings = earningsData, Expenses = expensesData });
        }
    }
}
