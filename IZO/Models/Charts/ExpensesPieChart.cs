using IZO.Models.Expenses;
using IZO.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IZO.Models.Charts
{
    public class ExpensePieChart
    {
        public Dictionary<ExpenseCategory, decimal> ExpensesByCategory { get; set; } = new Dictionary<ExpenseCategory, decimal>();

        public static ExpensePieChart initChartData() // ------ HERE IS MOCK DATA FOR PIE CHART ------ //
        {

            var mockExpenses = new Dictionary<ExpenseCategory, Expense[]>
            {
                { ExpenseCategory.HOUSING, new Expense[]{ new Expense(1200, ExpenseCategory.HOUSING, DateTime.Now) } },
                { ExpenseCategory.UTILITIES, new Expense[]{new Expense(200, ExpenseCategory.UTILITIES, DateTime.Now) } },
                { ExpenseCategory.ENTERTAINMENT, new Expense[]{new Expense(300, ExpenseCategory.ENTERTAINMENT, DateTime.Now) } },
                { ExpenseCategory.GROCERIES, new Expense[]{new Expense(150, ExpenseCategory.GROCERIES, DateTime.Now) } },
                { ExpenseCategory.CLOTHES, new Expense[]{new Expense(500, ExpenseCategory.CLOTHES, DateTime.Now) } },
                { ExpenseCategory.MISC, new Expense[]{ new Expense(100, ExpenseCategory.MISC, DateTime.Now) } },
                { ExpenseCategory.TRAVEL,new Expense[]{ new Expense(800, ExpenseCategory.TRAVEL, DateTime.Now) } },
                { ExpenseCategory.HEALTH, new Expense[]{ new Expense(250, ExpenseCategory.HEALTH, DateTime.Now) } },
                { ExpenseCategory.GASTRONOMY, new Expense[]{ new Expense(350, ExpenseCategory.GASTRONOMY, DateTime.Now) } }
            };

            var expensesByCategoryFixed = ExpenseAccesorService.monthlyExpenses.fixedExpenses
                .GroupBy(expense => expense.Key)
                .ToDictionary(group => group.Key, group => group.Sum(expenses => expenses.Value.Sum(e => (decimal) e.moneySpent)));

            var expensesByCategory = ExpenseAccesorService.monthlyExpenses.dayToDayExpenses
                .GroupBy(expense => expense.Key)
                .ToDictionary(group => group.Key, group => group.Sum(expenses => expenses.Value.Sum(e => (decimal)e.moneySpent)));

            foreach(var row in expensesByCategoryFixed)
            {
                if (expensesByCategory.ContainsKey(row.Key))
                {
                    expensesByCategory[row.Key] = row.Value + expensesByCategory[row.Key];
                    continue;
                }

                expensesByCategory.Add(row.Key, row.Value);
            }

            return new ExpensePieChart { ExpensesByCategory = expensesByCategory };
        }
    }
}
