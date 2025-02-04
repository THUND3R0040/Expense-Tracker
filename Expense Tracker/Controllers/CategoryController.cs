using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            // return _context.Categories != null ?
            // View(await _context.Categories.ToListAsync()) :
            // Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            try{
                return View(await _categoryService.GetCategories());
            }
            catch(Exception ex){
                return Problem("Error: " + ex.Message);
            }
        }


        // GET: Category/AddOrEdit
        public async  Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Category());
            else
                return View(await _categoryService.GetCategory(id));
        }

        // POST: Category/AddOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.CategoryId == 0)
                    await _categoryService.AddCategory(category);
                else
                    await _categoryService.UpdateCategory(category, category.CategoryId);                
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_categoryService.GetCategories() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
            }
            var category = _categoryService.GetCategory(id);
            if (category != null)
            {
                await _categoryService.DeleteCategory(id);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
