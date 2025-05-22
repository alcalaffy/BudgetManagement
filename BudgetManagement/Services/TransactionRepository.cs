using BudgetManagement.IServices;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly string connectionString;
        public TransactionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("StoreConnection");
        }
        public async Task Create(Transaction transaction)
        {
            using var conn=new SqlConnection(connectionString);
            var id = await conn.QuerySingleAsync<int>("Transacciones_Insertar",
                new
                {
                    transaction.UsuarioId,
                    transaction.FechaTransaccion,
                    transaction.Monto,
                    transaction.Nota,
                    transaction.CategoriaId,
                    transaction.CuentaId,                                    
                },
                commandType: System.Data.CommandType.StoredProcedure);


            transaction.Id = id;
        }
    }
}
