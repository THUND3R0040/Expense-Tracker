using Microsoft.AspNetCore.Mvc;
using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Expense_Tracker.Controllers

{



    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;


        public TransactionController(ITransactionService transactionService, ICategoryService categoryService , ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
            
        }
        

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name ?? "Guest";
            ViewBag.Username = username;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _transactionService.GetTransactionsWithCategory(userId));
        }

        // GET: Transaction/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            var username = User.Identity.Name ?? "Guest";
            ViewBag.Username = username;
            await PopulateCategories();
            if (id == 0)
                return View(new Transaction());
            else
                return View(await _transactionService.GetTransaction(id));
        }

        // POST: Transaction/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Transaction transaction)
        {
            var username = User.Identity.Name ?? "Guest";
            ViewBag.Username = username;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                transaction.UserId = userId;
                Console.WriteLine("User ID: " + userId);
            if (ModelState.IsValid)
            {
                
                if (transaction.TransactionId == 0){
                    await _transactionService.AddTransaction(transaction);
                }
                else{
                    await _transactionService.UpdateTransaction(transaction, transaction.TransactionId);
                }
                return RedirectToAction(nameof(Index));
            }
            await PopulateCategories();
            return View(transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var username = User.Identity.Name ?? "Guest";
            ViewBag.Username = username;
            if (_transactionService.GetTransactions() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
            }
            var transaction = _transactionService.GetTransaction(id);
            if (transaction != null)
            {
                await _transactionService.DeleteTransaction(id);
            }

            return RedirectToAction(nameof(Index));
        }


        [NonAction]
        public async Task PopulateCategories()
        {
            var CategoryCollection = await _categoryService.GetCategories();
            Category DefaultCategory = new Category() { CategoryId = 0, Title = "Choose a Category" };
            CategoryCollection = CategoryCollection.Prepend(DefaultCategory);
            ViewBag.Categories = CategoryCollection;
        }
    }
}
