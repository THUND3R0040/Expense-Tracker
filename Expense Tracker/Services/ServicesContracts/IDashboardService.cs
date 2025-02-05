using System.Security.Claims;
using Expense_Tracker.Models;

namespace Expense_Tracker.Services.ServicesContracts
{
    public interface IDashboardService
    {
        Task<List<Transaction>> GetTransactionsWithCategory(ClaimsPrincipal user);
        Task<int> GetTotalIncome(List<Transaction> selectedTransactions);
        Task<int> GetTotalExpense(List<Transaction> selectedTransactions);
        Task<int> GetBalance(int totalIncome, int totalExpense);
        Task<List<DoughnutChartDataDto>> GetDoughnutChartData(List<Transaction> selectedTransactions);
        Task<List<SplineChartDataDto>> GetIncomeChartData(List<Transaction> selectedTransactions);
        Task<List<SplineChartDataDto>> GetExpenseChartData(List<Transaction> selectedTransactions);
        Task<List<Transaction>> GetRecentTransactions(string userId);
    }
}