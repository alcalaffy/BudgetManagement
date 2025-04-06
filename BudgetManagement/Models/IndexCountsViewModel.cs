namespace BudgetManagement.Models
{
    public class IndexCountsViewModel
    {
        public string? TipoCuenta { get; set; }
        public IEnumerable<Count>?Cuentas { get; set; }
        public decimal Balance => Cuentas.Sum(c => c.Balance);

    }
}
