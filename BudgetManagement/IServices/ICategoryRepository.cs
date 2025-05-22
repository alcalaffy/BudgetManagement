using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ICategoryRepository
    {
        Task Create(Category category);
        Task Delete(int id, int userId);
        Task<Category> GetById(int Id, int userId);
        Task<IEnumerable<Category>> GetCategories(int userId);
        Task<IEnumerable<Category>> GetCategories(int userId, OperationType operationType);
        Task Update(Category category);
    }
}
