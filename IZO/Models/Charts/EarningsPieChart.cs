using IZO.Models.Earnings;
using IZO.Models.Expenses;
using IZO.Services;

namespace IZO.Models.Charts
{
    public class EarningsPieChart
    {
        public Dictionary<EarningCategory, decimal> EarningsByCategory { get; set; } = new Dictionary<EarningCategory, decimal>();

        public static EarningsPieChart initChartData() // ------ HERE IS MOCK DATA FOR PIE CHART ------ //
        {

           

            var earningsByCategoryFixed = EarningAccesorService.monthlyEarnings.fixed1Earnings
                .GroupBy(earning => earning.Key)
                .ToDictionary(group => group.Key, group => group.Sum(earnings => earnings.Value.Sum(e => (decimal)e.moneyEarn)));

            var earningsByCategory = EarningAccesorService.monthlyEarnings.dayToDayEarnings
                .GroupBy(earning => earning.Key)
                .ToDictionary(group => group.Key, group => group.Sum(earnings => earnings.Value.Sum(e => (decimal)e.moneyEarn)));

            foreach (var row in earningsByCategoryFixed)
            {
                if (earningsByCategory.ContainsKey(row.Key))
                {
                    earningsByCategory[row.Key] = row.Value + earningsByCategory[row.Key];
                    continue;
                }

                earningsByCategory.Add(row.Key, row.Value);
            }

            return new EarningsPieChart { EarningsByCategory = earningsByCategory };
        }
    }
}
