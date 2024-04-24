using IZO.Models;
using IZO.Models.Expenses;
using IZO.Services;
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

        public IActionResult AddExpense(string type, DateTime date, string category, double value)
        {
            // The add expense logic now resides here...
            if (Enum.TryParse(category, out ExpenseCategory expenseCategory))
            {
                Expense newExpense = new Expense
                {
                    Date = date,
                    category = expenseCategory,
                    moneySpent = value
                };

                _logger.LogInformation($"New expense added: {newExpense}");

                switch (type)
                {
                    case "FIXED":
                        if (ExpenseAccesorService.monthlyExpenses.fixedExpenses.ContainsKey(newExpense.category))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.fixedExpenses[newExpense.category].ToList();
                            expenses.Add(newExpense);
                            ExpenseAccesorService.monthlyExpenses.fixedExpenses[newExpense.category] = expenses.ToArray();
                            break;
                        }
                        ExpenseAccesorService.monthlyExpenses.fixedExpenses.Add(newExpense.category, new Expense[] {newExpense});

                        break;
                    case "D2D":
                        if (ExpenseAccesorService.monthlyExpenses.dayToDayExpenses.ContainsKey(newExpense.category))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.dayToDayExpenses[newExpense.category].ToList();
                            expenses.Add(newExpense);
                            ExpenseAccesorService.monthlyExpenses.dayToDayExpenses[newExpense.category] = expenses.ToArray();
                            break;
                        }
                        ExpenseAccesorService.monthlyExpenses.dayToDayExpenses.Add(newExpense.category, new Expense[] { newExpense });
                        break;

                }
                ExpenseAccesorService.saveExpenses();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }

        public IActionResult DeleteExpense(string type, DateTime date, string category, double value)
        {
            // The add expense logic now resides here...
            if (Enum.TryParse(category, out ExpenseCategory expenseCategory))
            {

                switch (type)
                {
                    case "FIXED":
                        if (ExpenseAccesorService.monthlyExpenses.fixedExpenses.ContainsKey(expenseCategory))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.fixedExpenses[expenseCategory].ToList();
                            expenses.Remove(expenses.Single( expense => expense.category == expenseCategory && expense.moneySpent == value && expense.Date == date));
                            ExpenseAccesorService.monthlyExpenses.fixedExpenses[expenseCategory] = expenses.ToArray();
                        }
                        break;
                    case "D2D":
                        if (ExpenseAccesorService.monthlyExpenses.dayToDayExpenses.ContainsKey(expenseCategory))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.dayToDayExpenses[expenseCategory].ToList();
                            expenses.Remove(expenses.Single(expense => expense.category == expenseCategory && expense.moneySpent == value && expense.Date == date));
                            ExpenseAccesorService.monthlyExpenses.dayToDayExpenses[expenseCategory] = expenses.ToArray();
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
