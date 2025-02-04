using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Expense_Tracker.Services.ServicesContracts;
using Expense_Tracker.Services.Services;

namespace Expense_Tracker.Controllers
{
    public class DashboardController : Controller
    {

        // private readonly ApplicationDbContext _context;
        

        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ILogger<DashboardController> logger , IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }
        
        public async Task<ActionResult> Index()
        {
            //Last 7 Days
            // DateTime StartDate = DateTime.Today.AddDays(-6);
            // DateTime EndDate = DateTime.Today;

            // List<Transaction> SelectedTransactions = await _context.Transactions
            //     .Include(x => x.Category)
            //     .Where(y => y.Date >= StartDate && y.Date <= EndDate)
            //     .ToListAsync();

            // List<Transaction> SelectedTransactions = await _context.Transactions
            // .FromSqlRaw("SELECT * FROM Transactions WHERE Date BETWEEN {0} AND {1}", StartDate, EndDate)
            // .Include(x => x.Category).ToListAsync();

            List<Transaction> SelectedTransactions = await _dashboardService.GetTransactionsWithCategory();
            

            //Total Income
            int TotalIncome = await _dashboardService.GetTotalIncome(SelectedTransactions);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");
            //Total Expense
            int TotalExpense = await _dashboardService.GetTotalExpense(SelectedTransactions);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance
            int Balance = await _dashboardService.GetBalance(TotalIncome, TotalExpense);
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);


            //Doughnut Chart - Expense By Category
            List<DoughnutChartDataDto> DoughnutChartData = await _dashboardService.GetDoughnutChartData(SelectedTransactions);
            
            ViewBag.DoughnutChartData = DoughnutChartData;
           
            

            //Income
            List<SplineChartDataDto> IncomeSummary = await _dashboardService.GetIncomeChartData(SelectedTransactions);

            //Expense
            List<SplineChartDataDto> ExpenseSummary = await _dashboardService.GetExpenseChartData(SelectedTransactions);

            //Combine Income & Expense
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-6).AddDays(i).ToString("dd-MMM"))
                .ToArray();

            ViewBag.SplineChartData = from day in Last7Days
                join income in IncomeSummary on day equals income.day into dayIncomeJoined
                from income in dayIncomeJoined.DefaultIfEmpty()
                join expense in ExpenseSummary on day equals expense.day into expenseJoined
                from expense in expenseJoined.DefaultIfEmpty()
                select new
                {
                    day = day,
                    income = income == null ? 0 : income.income,
                    expense = expense == null ? 0 : expense.expense,
                };
            //Recent Transactions
            ViewBag.RecentTransactions = await _dashboardService.GetRecentTransactions();


            return View();
        }
    }

    
}
