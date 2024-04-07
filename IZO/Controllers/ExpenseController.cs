using IZO.Models;
using IZO.Models.Expenses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IZO.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(ILogger<ExpenseController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddExpense(DateTime date, string category, double value)
        {
            // The add expense logic now resides here.
            if (Enum.TryParse(category, out ExpenseCategory expenseCategory))
            {
                var newExpense = new Expense
                {
                    Date = date,
                    category = expenseCategory,
                    moneySpent = value
                };

                _logger.LogInformation($"New expense added: {newExpense}");

                // Add expense to the data store...

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }
    }
}
