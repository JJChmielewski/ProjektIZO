using System.Text.Json;
using IZO.Controllers;
using IZO.Models.Expenses;
using IZO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class ExpenseControllerTests : IDisposable
    {
        private const string TestDirectory = "TestDirectory/";

        public ExpenseControllerTests()
        {
            Directory.CreateDirectory(TestDirectory);
        }

        public void Dispose()
        {
            Directory.Delete(TestDirectory, true);
        }

        [Fact]
        public void SaveExpenses_CreatesFile()
        {
            var monthlyExpenses = new MonthlyExpenses();
            ExpenseAccesorService.pathToSavedDir = TestDirectory;
            var expectedFilePath = Path.Combine(TestDirectory, $"{DateTime.Now.Month}-{DateTime.Now.Year}.json");

            ExpenseAccesorService.monthlyExpenses = monthlyExpenses;
            ExpenseAccesorService.saveExpenses();

            Assert.True(File.Exists(expectedFilePath));
        }

        [Fact]
        public void LoadExpenses_ReturnsMonthlyExpenses_WhenFileExists()
        {
            var expectedFilePath = Path.Combine(TestDirectory, "test.json");
            var monthlyExpenses = new MonthlyExpenses();
            monthlyExpenses.fixedExpenses.Add(ExpenseCategory.GROCERIES, new[] { new Expense { moneySpent = 50, category = ExpenseCategory.GROCERIES } });
            File.WriteAllText(expectedFilePath, JsonSerializer.Serialize(monthlyExpenses));

            var loadedExpenses = ExpenseAccesorService.loadExpenses(expectedFilePath);

            Assert.NotNull(loadedExpenses);
            Assert.Single(loadedExpenses.fixedExpenses);
            Assert.Equal(50, loadedExpenses.fixedExpenses[ExpenseCategory.GROCERIES][0].moneySpent);
        }

        [Fact]
        public void LoadExpenses_ReturnsNewMonthlyExpenses_WhenFileDoesNotExist()
        {
            var filePath = Path.Combine(TestDirectory, "nonexistent.json");

            var loadedExpenses = ExpenseAccesorService.loadExpenses(filePath);

            Assert.NotNull(loadedExpenses);
            Assert.Empty(loadedExpenses.fixedExpenses);
        }
    }
}