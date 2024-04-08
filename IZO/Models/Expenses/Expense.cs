using System.Globalization;
using System;

namespace IZO.Models.Expenses
{
    public class Expense
    {
        public double moneySpent { get; set; } = 0;  // value, how much money was spend during specific transaction
        public ExpenseCategory category { get; set; } = ExpenseCategory.MISC; // category of expense 
        public DateTime Date { get; set; } = DateTime.Now;  // Date of expense

        public Expense() { }

        public Expense(double moneySpent)
        {
            this.moneySpent = moneySpent;
        }

        public Expense(double moneySpent, ExpenseCategory category, DateTime date)
        {
            this.moneySpent = moneySpent;
            this.category = category;
            this.Date = date;
        }
        public override string ToString() //Displays "Today" instead of actual date when transaction happend on current date
        {
            string dateString = Date.Date == DateTime.Today ? "today" : Date.ToString(CultureInfo.CurrentCulture);
            return $"Category: {category}, Money Spent: {moneySpent}, Date: {dateString}";
        }
    }
}
