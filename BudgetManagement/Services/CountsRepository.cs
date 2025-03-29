using BudgetManagement.IServices;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class CountsRepository:ICountsRepository
    {
        private readonly string connectionString;
        public CountsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("StoreConnection");
        }
        public async Task Create(CreateCountViewModel count)
        {
            using var conn = new SqlConnection(connectionString);
            var id = await conn.QuerySingleAsync<int>(@"INSERT INTO Cuentas
                                                      (Nombre,TipoCuentaId,Balance,
                                                      Descripcion) 
                                                      VALUES (@Nombre,@TipoCuentaId,
                                                      @Balance,@Descripcion);
                                                      SELECT SCOPE_IDENTITY();", count);                                                  
            count.Id = id;
        }
    }
}
