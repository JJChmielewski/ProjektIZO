using IZO.Models.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IZO.Models.Charts
{
    public class ExpensePieChart
    {
        public Dictionary<ExpenseCategory, decimal> ExpensesByCategory { get; set; } = new Dictionary<ExpenseCategory, decimal>();

        public static ExpensePieChart GetMockData() // ------ HERE IS MOCK DATA FOR PIE CHART ------ //
        {

            var mockExpenses = new Dictionary<ExpenseCategory, Expense>
            {
                { ExpenseCategory.HOUSING, new Expense(1200, ExpenseCategory.HOUSING, DateTime.Now) },
                { ExpenseCategory.UTILITIES, new Expense(200, ExpenseCategory.UTILITIES, DateTime.Now) },
                { ExpenseCategory.ENTERTAINMENT, new Expense(300, ExpenseCategory.ENTERTAINMENT, DateTime.Now) },
                { ExpenseCategory.GROCERIES, new Expense(150, ExpenseCategory.GROCERIES, DateTime.Now) },
                { ExpenseCategory.CLOTHES, new Expense(500, ExpenseCategory.CLOTHES, DateTime.Now) },
                { ExpenseCategory.MISC, new Expense(100, ExpenseCategory.MISC, DateTime.Now) },
                { ExpenseCategory.TRAVEL, new Expense(800, ExpenseCategory.TRAVEL, DateTime.Now) },
                { ExpenseCategory.HEALTH, new Expense(250, ExpenseCategory.HEALTH, DateTime.Now) },
                { ExpenseCategory.GASTRONOMY, new Expense(350, ExpenseCategory.GASTRONOMY, DateTime.Now) }
            };

            var expensesByCategory = mockExpenses
                .GroupBy(expense => expense.Key)
                .ToDictionary(group => group.Key, group => group.Sum(expense => Convert.ToDecimal(expense.Value.moneySpent)));

            return new ExpensePieChart { ExpensesByCategory = expensesByCategory };
        }
    }
}
