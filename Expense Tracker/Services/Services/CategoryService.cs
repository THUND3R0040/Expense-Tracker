using Expense_Tracker.Models;
using Expense_Tracker.Services.ServicesContracts;
using Expense_Tracker.Repositories;

namespace Expense_Tracker.Services.Services
{
 public class CategoryService : ICategoryService {
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository) {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IEnumerable<Category>> GetCategories() {
        return await _categoryRepository.GetAllAsync();
    }


    public async Task<Category> GetCategory(int id) {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task AddCategory(Category category) {
        await _categoryRepository.AddAsync(category);
    }

    public async Task UpdateCategory(Category category, object key) {
        await _categoryRepository.UpdateAsync(category, key);
    }

    public async Task DeleteCategory(object key) {
        await _categoryRepository.DeleteAsync(key);
 }

    
 }

}