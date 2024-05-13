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
                    case "EARNING":
                        if (ExpenseAccesorService.monthlyExpenses.earnings.ContainsKey(newExpense.category))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.earnings[newExpense.category].ToList();
                            expenses.Add(newExpense);
                            ExpenseAccesorService.monthlyExpenses.earnings[newExpense.category] = expenses.ToArray();
                            break;
                        }
                        ExpenseAccesorService.monthlyExpenses.earnings.Add(newExpense.category, new Expense[] { newExpense });
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
                            expenses.Remove(expenses.First( expense => expense.category == expenseCategory && expense.moneySpent == value && expense.Date == date));
                            ExpenseAccesorService.monthlyExpenses.fixedExpenses[expenseCategory] = expenses.ToArray();
                        }
                        break;
                    case "D2D":
                        if (ExpenseAccesorService.monthlyExpenses.dayToDayExpenses.ContainsKey(expenseCategory))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.dayToDayExpenses[expenseCategory].ToList();
                            expenses.Remove(expenses.First(expense => expense.category == expenseCategory && expense.moneySpent == value && expense.Date == date));
                            ExpenseAccesorService.monthlyExpenses.dayToDayExpenses[expenseCategory] = expenses.ToArray();
                        }
                        break;
                    case "EARNING":
                        if (ExpenseAccesorService.monthlyExpenses.earnings.ContainsKey(expenseCategory))
                        {
                            List<Expense> expenses = ExpenseAccesorService.monthlyExpenses.earnings[expenseCategory].ToList();
                            expenses.Remove(expenses.First(expense => expense.category == expenseCategory && expense.moneySpent == value && expense.Date == date));
                            ExpenseAccesorService.monthlyExpenses.earnings[expenseCategory] = expenses.ToArray();
                        }
                        break;

                }
                ExpenseAccesorService.saveExpenses();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid category." });
        }

        public IActionResult GeneratePlan(double value)
        {

            var currentDate = DateTime.Now;

            double expenses = 0;
            double earnings = 0;

            foreach (var expense in ExpenseAccesorService.monthlyExpenses.fixedExpenses.Values)
            {
                expenses += expense.Sum(e => e.moneySpent);
            }

            foreach (var expense in ExpenseAccesorService.monthlyExpenses.dayToDayExpenses.Values)
            {
                expenses += expense.Sum(e => e.moneySpent);
            }

            foreach (var earning in ExpenseAccesorService.monthlyExpenses.earnings.Values)
            {
                earnings += earning.Sum(e => e.moneySpent);
            }

            var failJson = Json(new { success = true, planedSaving = 99999 });

            if (earnings - expenses < 0)
            {
                return failJson;
            }

            int months = (int)(value / (earnings - expenses));

            if (months < 0) {
                return failJson;
            }

            return Json(new { success = true, planedSaving = months });


        }
    }
}
