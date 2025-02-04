using Expense_Tracker.Models;

namespace Expense_Tracker.Services.ServicesContracts
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetTransactions();
        Task<Transaction> GetTransaction(int id);
        Task AddTransaction(Transaction transaction);
        Task UpdateTransaction(Transaction transaction , object key);
        Task DeleteTransaction(int id);

        Task <IEnumerable<Transaction>> GetTransactionsWithCategory();
    }
}