using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface  ICountTypesRepository
    {
        Task Create(CountType countType);
        Task Delete(int id);
        Task<IEnumerable<CountType>> Get(int usuarioId);
        Task<CountType> GetCountTypeById(int id, int usuarioId);
        Task Order(IEnumerable<CountType> countTypes);
        Task Update(CountType countType);
        Task<bool> Validate(string name, int userId);

    }
}
