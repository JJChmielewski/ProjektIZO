
namespace IZO.Models.Expenses
{
    public class MonthlyExpenses
    {
        public Dictionary<ExpenseCategory, Expense[]> fixedExpenses { get; set; } = new Dictionary<ExpenseCategory, Expense[]>();
        public Dictionary<ExpenseCategory, Expense[]> dayToDayExpenses { get; set; } = new Dictionary<ExpenseCategory, Expense[]>();

        public Dictionary<ExpenseCategory, Expense[]> earnings { get; set; } = new Dictionary<ExpenseCategory, Expense[]>();

    }

}
