using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;
using System.Threading.Tasks;

namespace Expense_Tracker.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ICategoryService _categoryService;

        public TransactionController(ITransactionService transactionService, ICategoryService categoryService)
        {
            _transactionService = transactionService;
            _categoryService = categoryService;
        }
        

        // GET: Transaction
        public async Task<IActionResult> Index()
        {
            return View(await _transactionService.GetTransactionsWithCategory());
        }

        // GET: Transaction/AddOrEdit
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            await PopulateCategories();
            if (id == 0)
                return View(new Transaction());
            else
                return View(await _transactionService.GetTransaction(id));
        }

        // POST: Transaction/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                if (transaction.TransactionId == 0)
                    await _transactionService.AddTransaction(transaction);
                else
                    await _transactionService.UpdateTransaction(transaction, transaction.TransactionId);
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
