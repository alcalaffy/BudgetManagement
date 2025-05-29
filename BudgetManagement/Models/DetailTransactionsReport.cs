namespace BudgetManagement.Models
{
    public class DetailTransactionsReport
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal BalanceDepositos => TransaccionesAgrupadas.Sum(x => x.BalanceDepositos);
        public decimal BalanceRetiros => TransaccionesAgrupadas.Sum(x=>x.BalanceRetiros);
        public IEnumerable<TransactionsByDate>TransaccionesAgrupadas { get; set; }
        public decimal Total => BalanceDepositos - BalanceRetiros;
        public class TransactionsByDate
        {
            public DateTime FechaTransaccion { get; set; }
            public IEnumerable<Transaction> Transacciones { get; set; }
            public decimal BalanceDepositos => Transacciones.Where(t => t.TipoOperacionId == OperationType.Income)
                                                            .Sum(m => m.Monto);
            public decimal BalanceRetiros => Transacciones.Where(t => t.TipoOperacionId == OperationType.Outcome)
                                                          .Sum(m => m.Monto);
        }
                                   
    }
}
