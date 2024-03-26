namespace IZO.Models.Expenses
{
    public class Expense
    {
        public double moneySpent { get; set; } = 0;
        public ExpenseCategory category { get; set; } = ExpenseCategory.MISC;
    }
}
