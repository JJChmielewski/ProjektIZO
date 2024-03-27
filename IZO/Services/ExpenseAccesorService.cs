using IZO.Models.Expenses;
using System;
using System.IO;
using System.Text.Json;

namespace IZO.Services
{
    public static class ExpenseAccesorService
    {
        public static string pathToSavedDir = "saved/";
        public static MonthlyExpenses monthlyExpenses;

        public static void saveExpenses()
        {
            string file = getCurrentFilePath();
            if (!Directory.Exists(pathToSavedDir)) { 
                Directory.CreateDirectory(pathToSavedDir);
            }

            if (!File.Exists(file))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(file))
                {
                    sw.Write(JsonSerializer.Serialize(monthlyExpenses));
                }
                return;
            }

            using (StreamWriter outputFile = new StreamWriter(getCurrentFilePath(), false))
            {
                outputFile.Write(JsonSerializer.Serialize(monthlyExpenses));
            }
        }

        public static void loadExpenses()
        {
            string file = getCurrentFilePath();
            if (File.Exists(file))
            {
                MonthlyExpenses? tempExpenses;
                using (FileStream openStream = File.OpenRead(file))
                {
                    tempExpenses = JsonSerializer.Deserialize<MonthlyExpenses>(openStream);
                }

                if (tempExpenses != null)
                {
                    monthlyExpenses = tempExpenses;
                    return;
                }
            }

            monthlyExpenses = new MonthlyExpenses();
            saveExpenses();
        }

        private static string getCurrentFilePath()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            return Path.Combine(pathToSavedDir, String.Format("{0}-{1}.json", month, year));
        }

    }
}
