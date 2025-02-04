using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Repositories{
    public interface IDashboardRepository : IGenericRepository<Category>
    {
        Task<List<Transaction>> GetTransactionsWithCategory();
        Task<int> GetTotalIncome(List<Transaction> selectedTransactions);
        Task<int> GetTotalExpense(List<Transaction> selectedTransactions);
        Task<int> GetBalance(int totalIncome, int totalExpense);
        Task<List<DoughnutChartDataDto>> GetDoughnutChartData(List<Transaction> selectedTransactions);
        Task<List<SplineChartDataDto>> GetIncomeChartData(List<Transaction> selectedTransactions);
        Task<List<SplineChartDataDto>> GetExpenseChartData(List<Transaction> selectedTransactions);
        Task<List<Transaction>> GetRecentTransactions();
    }

    public class DashboardRepository : GenericRepository<Category>, IDashboardRepository
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

        public async Task<int> GetTotalIncome(List<Transaction> selectedTransactions)
        {
            int TotalIncome = selectedTransactions
            .Where(i => i.Category.Type == "Income")
            .Sum(j => j.Amount);
            return TotalIncome;
        }

        public async Task<int> GetTotalExpense(List<Transaction> selectedTransactions)
        {
            int TotalExpense = selectedTransactions
            .Where(i => i.Category.Type == "Expense")
            .Sum(j => j.Amount);
            return TotalExpense;
        }

        public async Task<int> GetBalance(int totalIncome, int totalExpense)
        {
            int Balance = totalIncome - totalExpense;
            return Balance;
        }

        public async Task<List<DoughnutChartDataDto>> GetDoughnutChartData(List<Transaction> selectedTransactions)
        {
            List<DoughnutChartDataDto> DoughnutChartData = selectedTransactions
            .Where(i => i.Category.Type == "Expense")
            .GroupBy(x => x.Category.Title)
            .Select(y => new DoughnutChartDataDto
            {
                categoryTitleWithIcon = y.Key,
                amount = y.Sum(z => z.Amount),
                formattedAmount = y.Sum(z => z.Amount).ToString("C0")
            }).ToList();
            return DoughnutChartData;
        }

        public async Task<List<SplineChartDataDto>> GetIncomeChartData(List<Transaction> selectedTransactions)
        {
            List<SplineChartDataDto> IncomeSummary = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartDataDto()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();
            return IncomeSummary;
        }

        public async Task<List<SplineChartDataDto>> GetExpenseChartData(List<Transaction> selectedTransactions)
        {
            List<SplineChartDataDto> ExpenseSummary = selectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartDataDto()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();
            return ExpenseSummary;
        }

        public async Task<List<Transaction>> GetRecentTransactions()
        {
            List<Transaction> RecentTransactions = await _context.Transactions
            .Include(x => x.Category)
            .OrderByDescending(y => y.Date)
            .Take(5)
            .ToListAsync();
            return RecentTransactions;
        }


    }


        
    }
