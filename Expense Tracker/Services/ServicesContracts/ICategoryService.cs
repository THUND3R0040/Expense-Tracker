using Expense_Tracker.Models;

namespace Expense_Tracker.Services.ServicesContracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category , object key);
        Task DeleteCategory(object key);
    }
}

