using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ICountsRepository
    {
        Task Create(Count count);
        Task Delete(int id);
        Task<Count> GetCountById(int id, int usuarioId);
        Task<IEnumerable<Count>> SearchCounts(int usuarioId);
        Task Update(CreateCountViewModel count);
    }
}
