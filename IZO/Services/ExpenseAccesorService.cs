using IZO.Models.Expenses;
using System.Text.Json;

namespace IZO.Services
{
    public static class ExpenseAccesorService
    {
        public static string pathToSavedDir = "saved/";
        public static MonthlyExpenses monthlyExpenses = loadExpenses(getCurrentFilePath());

        public static void saveExpenses()
        {
            string file = getCurrentFilePath();
            if (!Directory.Exists(pathToSavedDir)) { 
                Directory.CreateDirectory(pathToSavedDir);
            }

            using (StreamWriter sw = File.CreateText(file))
            {
                sw.Write(JsonSerializer.Serialize(monthlyExpenses));
            }
        }

        public static MonthlyExpenses loadExpenses(string file)
        {
            MonthlyExpenses? tempExpenses;
            if (File.Exists(file))
            {
                using (FileStream openStream = File.OpenRead(file))
                {
                    tempExpenses = JsonSerializer.Deserialize<MonthlyExpenses>(openStream);
                }

                if (tempExpenses != null)
                {
                    return tempExpenses;
                }
            }

            tempExpenses = new MonthlyExpenses();
            tempExpenses.fixedExpenses = getPastFixedExpenses();
            return tempExpenses;
        }

        private static string getCurrentFilePath()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            return Path.Combine(pathToSavedDir, String.Format("{0}-{1}.json", month, year));
        }

        private static Dictionary<ExpenseCategory, Expense> getPastFixedExpenses()
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

                    string[] fileSplit = file.Replace(".json", "").Replace(pathToSavedDir,"").Split('-');
                    string[] newestSplit = newest.Replace(".json", "").Replace(pathToSavedDir, "").Split('-');
                    
                    if (int.Parse(fileSplit[1]) > int.Parse(newestSplit[1]) || 
                        (int.Parse(fileSplit[1]) == int.Parse(newestSplit[1]) && int.Parse(fileSplit[0]) > int.Parse(newestSplit[0])))
                    {
                        newest = file;
                    }
                }

                if (newest != null)
                {
                    return loadExpenses(newest).fixedExpenses;
                }
            }

            return new Dictionary<ExpenseCategory, Expense>();
        }

    }
}
