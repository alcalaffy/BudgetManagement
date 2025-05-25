
namespace BudgetManagement.Models
{
    public class UpdateTransactionViewModel:CreateTransactionViewModel
    {
        public int CuentaAnterior { get; set; }
        public decimal MontoAnterior { get; set; }
    }
}
