using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface  ICountTypesRepository
    {
        Task Create(CountType countType);
        Task<IEnumerable<CountType>> Get(int usuarioId);
        Task<CountType> GetCountTypeById(string nombre, int usuarioId);
        Task Update(CountType countType);
        Task<bool> Validate(string name, int userId);

    }
}
