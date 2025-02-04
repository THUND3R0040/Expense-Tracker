using Expense_Tracker.Models;

namespace Expense_Tracker.Services.ServicesContracts
{
    public interface IDashboardService
    {
        Task<List<Transaction>> GetTransactionsWithCategory();
    }
}