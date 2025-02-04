using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;
using Expense_Tracker.Repositories;

namespace Expense_Tracker.Services.Services
{
 public class TransactionService : ITransactionService {
    private readonly ITransactionRepository _transactionRepository;
    public TransactionService(ITransactionRepository transactionRepository) {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<IEnumerable<Transaction>> GetTransactions() {
        return await _transactionRepository.GetAllAsync();
    }


    public async Task<Transaction> GetTransaction(int id) {
        return await _transactionRepository.GetByIdAsync(id);
    }

    public async Task AddTransaction(Transaction transaction) {
        await _transactionRepository.AddAsync(transaction);
    }

    public async Task UpdateTransaction(Transaction transaction , object key) {
        await _transactionRepository.UpdateAsync(transaction , key);
    }

    public async Task DeleteTransaction(int id) {
        await _transactionRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsWithCategory() {
        return await _transactionRepository.GetTransactionsWithCategory();
    }

    }
}