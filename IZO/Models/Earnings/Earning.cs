using IZO.Models.Expenses;
using System.Globalization;

namespace IZO.Models.Earnings
{
    public class Earning
    {
        public double moneyEarn { get; set; } = 0;  // value, how much money was spend during specific transaction
        public EarningCategory category { get; set; } = EarningCategory.EMPLOYMENT; // category of expense 
        public DateTime Date { get; set; } = DateTime.Now;  // Date of expense

        public Earning() { }

        public Earning(double moneySpent)
        {
            this.moneyEarn = moneyEarn;
        }

        public Earning(double moneyEarn, EarningCategory category, DateTime date)
        {
            this.moneyEarn = moneyEarn;
            this.category = category;
            this.Date = date;
        }
        public override string ToString() //Displays "Today" instead of actual date when transaction happend on current date
        {
            string dateString = Date.Date == DateTime.Today ? "today" : Date.ToString(CultureInfo.CurrentCulture);
            return $"Category: {category}, Money Spent: {moneyEarn}, Date: {dateString}";
        }
    }
}
