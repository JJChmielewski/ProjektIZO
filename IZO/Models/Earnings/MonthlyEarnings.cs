namespace IZO.Models.Earnings
{
    public class MonthlyEarnings
    {
        public Dictionary<EarningCategory, Earning[]> fixed1Earnings { get; set; } = new Dictionary<EarningCategory, Earning[]>();
        public Dictionary<EarningCategory, Earning[]> dayToDayEarnings { get; set; } = new Dictionary<EarningCategory, Earning[]>();

    }
}
