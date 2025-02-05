using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Syncfusion.EJ2.Linq;

namespace Expense_Tracker.Repositories{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsWithCategory(string userId);
    }

    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        public TransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsWithCategory(string userId)
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Category).Where(t=>t.UserId == userId);
            return await applicationDbContext.ToListAsync();
        }
    }
}