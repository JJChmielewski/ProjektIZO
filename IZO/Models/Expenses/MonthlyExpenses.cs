namespace IZO.Models.Expenses
{
    public class MonthlyExpenses
    {
        public Dictionary<ExpenseCategory, Expense> fixedExpenses { get; set; } = new Dictionary<ExpenseCategory, Expense>();
        public Dictionary<ExpenseCategory, Expense> dayToDayExpenses { get; set; } = new Dictionary<ExpenseCategory, Expense>();

        public static MonthlyExpenses GetMockData() // ------ HERE IS MOCK DATA FOR TABLES ------ //
        {

            var mockMonthlyExpenses = new MonthlyExpenses
            {
                fixedExpenses = new Dictionary<ExpenseCategory, Expense>
                {
                    { ExpenseCategory.HOUSING, new Expense(1200, ExpenseCategory.HOUSING, DateTime.Now) },
                    { ExpenseCategory.UTILITIES, new Expense(200, ExpenseCategory.UTILITIES, DateTime.Now) }

                },
                dayToDayExpenses = new Dictionary<ExpenseCategory, Expense>
                {
                    { ExpenseCategory.ENTERTAINMENT, new Expense(300, ExpenseCategory.ENTERTAINMENT, DateTime.Now) },
                    { ExpenseCategory.GROCERIES, new Expense(150, ExpenseCategory.GROCERIES, DateTime.Now) },
                    { ExpenseCategory.TRAVEL, new Expense(50, ExpenseCategory.TRAVEL, DateTime.Now) },
                    { ExpenseCategory.CLOTHES, new Expense(500, ExpenseCategory.CLOTHES, DateTime.Now) }
                }
            };

            return mockMonthlyExpenses;
        }
    }

}
