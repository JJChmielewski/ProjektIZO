using IZO.Models.Charts;
using IZO.Models.Expenses;

namespace IZO.Models.ViewModels
{
    public class ExpensesViewModel
    {
        public MonthlyExpenses MonthlyExpenses { get; set; }
        public ExpensePieChart ExpensePieChart { get; set; }
    }
}