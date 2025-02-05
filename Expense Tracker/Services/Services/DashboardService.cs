using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;
using Expense_Tracker.Repositories;
using System.Security.Claims;

namespace Expense_Tracker.Services.Services
{
 public class DashboardService : IDashboardService {
    private readonly IDashboardRepository _dashboardRepository;
    public DashboardService(IDashboardRepository dashboardRepository) {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<List<Transaction>> GetTransactionsWithCategory(ClaimsPrincipal user){
        return await _dashboardRepository.GetTransactionsWithCategory(user);
    }

    public async Task<int> GetTotalIncome(List<Transaction> selectedTransactions){
        return await _dashboardRepository.GetTotalIncome(selectedTransactions);
    }

    public async Task<int> GetTotalExpense(List<Transaction> selectedTransactions){
        return await _dashboardRepository.GetTotalExpense(selectedTransactions);
    }

    public async Task<int> GetBalance(int totalIncome, int totalExpense){
        return await _dashboardRepository.GetBalance(totalIncome, totalExpense);
    }

    public async Task<List<DoughnutChartDataDto>> GetDoughnutChartData(List<Transaction> selectedTransactions){
        return await _dashboardRepository.GetDoughnutChartData(selectedTransactions);
    }
    
    public async Task<List<SplineChartDataDto>> GetIncomeChartData(List<Transaction> selectedTransactions){
        return await _dashboardRepository.GetIncomeChartData(selectedTransactions);
    }

    public async Task<List<SplineChartDataDto>> GetExpenseChartData(List<Transaction> selectedTransactions){
        return await _dashboardRepository.GetExpenseChartData(selectedTransactions);
    }

    public async Task<List<Transaction>> GetRecentTransactions(string userId){
        return await _dashboardRepository.GetRecentTransactions(userId);
    }


    }
}