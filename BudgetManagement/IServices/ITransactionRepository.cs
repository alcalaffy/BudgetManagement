using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
    }
}
