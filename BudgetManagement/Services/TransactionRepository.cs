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
        public async Task Update(Transaction transaction,int cuentaAnteriorid, decimal montoAnterior)
        {
            using var conn = new SqlConnection(connectionString);
            await conn.ExecuteAsync("Transacciones_Actualizar", new
            {
                transaction.Id,
                transaction.FechaTransaccion,
                transaction.Monto,
                montoAnterior,
                transaction.CuentaId,
                cuentaAnteriorid,
                transaction.CategoriaId,
                transaction.Nota
            },
            commandType: System.Data.CommandType.StoredProcedure);
            
        }
        public async Task<Transaction> GetById(int id,int userId)
        {
            using var conn = new SqlConnection(connectionString);
            var transaction = await conn.QueryFirstOrDefaultAsync<Transaction>(@"SELECT Transacciones.*, cat.TipoOperacionId                                                                            
                                                                                FROM Transacciones
                                                                                INNER JOIN Categorias cat ON cat.Id = Transacciones.CategoriaId 
                                                                                WHERE Transacciones.Id = @Id
                                                                                AND Transacciones.UsuarioId = @userId", new {id, userId });
            return transaction;
        }
        public async Task Delete(int id)
        {
            using var conn = new SqlConnection(connectionString);
            await conn.ExecuteAsync("Transacciones_Borrar", new { id },
                                     commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<Transaction>>GetTransactionByAcount(GetTransactionsByAcount acount)
        {
            using var conn = new SqlConnection(connectionString);
            var transactions = await conn.QueryAsync<Transaction>(@"SELECT t.Id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria,
                                                                    cu.Nombre as Cuenta, c.TipoOperacionId
                                                                    FROM Transacciones t
                                                                    INNER JOIN Categorias c
                                                                    ON c.Id = t.CategoriaId
                                                                    INNER JOIN Cuentas cu
                                                                    ON cu.Id = t.CuentaId
                                                                    WHERE t.CuentaId = @CuentaId AND t.UsuarioId = @UsuarioId
                                                                    AND FechaTransaccion BETWEEN @FechaInicio AND @FechaFin",acount);
            return transactions; 
        }
    }
}
