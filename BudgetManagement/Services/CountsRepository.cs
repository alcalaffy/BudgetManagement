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
        public async Task Create(Count count)
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
        public async Task<IEnumerable<Count>> SearchCounts(int usuarioId)
        {
            using var conn = new SqlConnection(connectionString);
            var counts = await conn.QueryAsync<Count>(@"SELECT Cuentas.Id,Cuentas.Nombre,Balance,TC.NOMBRE AS TipoCuenta
                                                        FROM CUENTAS INNER JOIN TiposCuentas TC ON TC.Id=Cuentas.TipoCuentaId 
                                                        WHERE TC.UsuarioId=@UsuarioId 
                                                        ORDER BY TC.ORDEN", new { usuarioId });
            return counts;
        }
        public async Task<Count> GetCountById(int id, int usuarioId)
        {
            using var conn = new SqlConnection(connectionString);

            const string sql = @"
                                SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, Cuentas.TipoCuentaId
                                FROM CUENTAS
                                INNER JOIN TiposCuentas TC ON TC.Id = Cuentas.TipoCuentaId 
                                WHERE TC.UsuarioId = @UsuarioId AND Cuentas.Id = @Id";

            return await conn.QueryFirstOrDefaultAsync<Count>(sql, new { Id = id, UsuarioId = usuarioId });
        }
        public async Task Update(CreateCountViewModel count)
        {
            using var conn = new SqlConnection(connectionString);

            await conn.ExecuteAsync(@"UPDATE Cuentas
                                    SET Nombre = @Nombre, Balance = @Balance, Descripcion = @Descripcion,
                                    TipoCuentaId = @TipoCuentaId
                                    WHERE Id = @Id", count);
        }
        public async Task Delete(int id)
        {
            using var conn = new SqlConnection(connectionString);
            await conn.ExecuteAsync(@"DELETE Cuentas WHERE Id = @Id", new {id});
        }

    }
}
