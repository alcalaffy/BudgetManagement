using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ICountsRepository
    {
        Task Create(Count count);
        Task<IEnumerable<Count>> SearchCounts(int usuarioId);
    }
}
