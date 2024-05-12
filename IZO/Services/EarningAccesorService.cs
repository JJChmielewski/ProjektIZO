using IZO.Models.Earnings;
using System.Text.Json;

namespace IZO.Services
{
    public class EarningAccesorService
    {
        public static string pathToSavedDir = "savedearnings/";
        public static MonthlyEarnings monthlyEarnings = loadEarnings(getCurrentFilePath());

        public static void saveEarnings()
        {
            string file = getCurrentFilePath();
            if (!Directory.Exists(pathToSavedDir))
            {
                Directory.CreateDirectory(pathToSavedDir);
            }

            using (StreamWriter sw = File.CreateText(file))
            {
                sw.Write(JsonSerializer.Serialize(monthlyEarnings));
            }
        }

        public static MonthlyEarnings loadEarnings(string file)
        {
            MonthlyEarnings? tempEarnings;
            if (File.Exists(file))
            {
                using (FileStream openStream = File.OpenRead(file))
                {
                    tempEarnings = JsonSerializer.Deserialize<MonthlyEarnings>(openStream);
                }

                if (tempEarnings != null)
                {
                    return tempEarnings;
                }
            }

            tempEarnings = new MonthlyEarnings();
            tempEarnings.fixed1Earnings = getPastFixed1Earnings();
            return tempEarnings;
        }

        private static string getCurrentFilePath()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            return Path.Combine(pathToSavedDir, String.Format("{0}-{1}.json", month, year));
        }

        private static Dictionary<EarningCategory, Earning[]> getPastFixed1Earnings()
        {
            if (Directory.Exists(pathToSavedDir))
            {
                string[] files = Directory.GetFiles(pathToSavedDir);
                string newest = null;

                foreach (string file in files)
                {
                    if (!file.Contains(".json"))
                    {
                        continue;
                    }

                    if (newest == null)
                    {
                        newest = file;
                        continue;
                    }

                    string[] fileSplit = file.Replace(".json", "").Replace(pathToSavedDir, "").Split('-');
                    string[] newestSplit = newest.Replace(".json", "").Replace(pathToSavedDir, "").Split('-');

                    if (int.Parse(fileSplit[1]) > int.Parse(newestSplit[1]) ||
                        (int.Parse(fileSplit[1]) == int.Parse(newestSplit[1]) && int.Parse(fileSplit[0]) > int.Parse(newestSplit[0])))
                    {
                        newest = file;
                    }
                }

                if (newest != null)
                {
                    return loadEarnings(newest).fixed1Earnings;
                }
            }

            return new Dictionary<EarningCategory, Earning[]>();
        }
    }
}
