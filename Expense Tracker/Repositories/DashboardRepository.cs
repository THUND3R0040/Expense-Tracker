using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Repositories{
    public interface IDashboardRepository : IGenericRepository<Category>
    {
        Task<List<Transaction>> GetTransactionsWithCategory();
        Task<int> GetTotalIncome(List<Transaction> selectedTransactions);
        Task<int> GetTotalExpense(List<Transaction> selectedTransactions);
        Task<int> GetBalance(int totalIncome, int totalExpense);
    }

    public class DashboardRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<Transaction>> GetTransactionsWithCategory()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;
            List<Transaction> SelectedTransactions = await _context.Transactions
            .FromSqlRaw("SELECT * FROM Transactions WHERE Date BETWEEN {0} AND {1}", StartDate, EndDate)
            .Include(x => x.Category).ToListAsync();
            return SelectedTransactions;
        }

        public 

        }

        
    }
