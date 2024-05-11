using IZO.Models.Charts;
using IZO.Models.Earnings;
using IZO.Models.Expenses;

namespace IZO.Models.ViewModels
{
    public class ExpensesViewModel
    {
        public MonthlyExpenses MonthlyExpenses { get; set; }
        public ExpensePieChart ExpensePieChart { get; set; }
    }
    public class EarningsViewModel
    {
        public MonthlyEarnings MonthlyExpenses { get; set; }
        public EarningsPieChart EarningPieChart { get; set; }
    }


}
