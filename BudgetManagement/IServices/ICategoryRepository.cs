using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ICategoryRepository
    {
        Task Create(Category category);
        Task<IEnumerable<Category>> GetCategories(int userId);
    }
}
