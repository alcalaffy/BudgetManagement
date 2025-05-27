using BudgetManagement.Models;

namespace BudgetManagement.IServices
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaction);
        Task Delete(int id);
        Task<Transaction> GetById(int id, int userId);
        Task<IEnumerable<Transaction>> GetTransactionByAcount(GetTransactionsByAcount acount);
        Task Update(Transaction transaction, int prevAccount, decimal prevAmount);
    }
}
