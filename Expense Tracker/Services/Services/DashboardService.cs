using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;
using Expense_Tracker.Repositories;

namespace Expense_Tracker.Services.Services
{
 public class DashboardService : IDashboardService {
    private readonly IDashboardRepository _dashboardRepository;
    public DashboardService(IDashboardRepository dashboardRepository) {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<List<Transaction>> GetTransactionsWithCategory(){
        return await _dashboardRepository.GetTransactionsWithCategory();
    }


    }
}