using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ICountsRepository
    {
        Task Create(CreateCountViewModel count);
    }
}
