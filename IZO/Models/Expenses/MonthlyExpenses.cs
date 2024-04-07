namespace IZO.Models.Expenses
{
    public class MonthlyExpenses
    {
        public Dictionary<ExpenseCategory, Expense> fixedExpenses { get; set; } = new Dictionary<ExpenseCategory, Expense>();
        public Dictionary<ExpenseCategory, Expense> dayToDayExpenses { get; set; } = new Dictionary<ExpenseCategory, Expense>();

        public static MonthlyExpenses GetMockData() // MOCK DATA 
        {
            var mockMonthlyExpenses = new MonthlyExpenses
            {
                fixedExpenses = new Dictionary<ExpenseCategory, Expense>
                {
                    { ExpenseCategory.HOUSING, new Expense(1200, ExpenseCategory.HOUSING, DateTime.Now) },
                    
                },
                dayToDayExpenses = new Dictionary<ExpenseCategory, Expense>
                {
                    { ExpenseCategory.ENTERTAINMENT, new Expense(300, ExpenseCategory.ENTERTAINMENT, DateTime.Now) },
                    { ExpenseCategory.GROCERIES, new Expense(150, ExpenseCategory.GROCERIES, DateTime.Now) },
                    { ExpenseCategory.TRANSIT, new Expense(50, ExpenseCategory.TRAVEL, DateTime.Now) }
                }
            };

            return mockMonthlyExpenses;
        }
    }

}
