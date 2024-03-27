namespace IZO.Models.Expenses
{
    public class Expense
    {
        public double moneySpent { get; set; } = 0;
        public ExpenseCategory category { get; set; } = ExpenseCategory.MISC;

        public Expense() { }

        public Expense(double moneySepnt)
        {
            this.moneySpent = moneySepnt;
        }

        public Expense(double moneySpent, ExpenseCategory category)
        {
            this.moneySpent = moneySpent;
            this.category = category;
        }
    }
}
