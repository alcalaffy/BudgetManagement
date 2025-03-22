

using BudgetManagement.IServices;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class CountTypesRepository: ICountTypesRepository
    {
        private readonly string connectionString;
        public CountTypesRepository(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("StoreConnection");
        }

        public async Task Create(CountType countType) 
        {
            using var conn=new SqlConnection(connectionString);
            var id = await conn.QuerySingleAsync<int>(@"INSERT INTO TiposCuentas(Nombre,UsuarioId,Orden) 
                                           VALUES(@Nombre,@UsuarioId,0) 
                                           SELECT SCOPE_IDENTITY()", countType
                                           );
            countType.Id = id;
        }

        public async Task<bool> Validate(string nombre,int usuarioId) 
        {
            using var conn = new SqlConnection(connectionString);
            var exist = await conn.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM TiposCuentas 
                                                          WHERE Nombre=@Nombre AND UsuarioId=@UsuarioId",
                                                          new { nombre, usuarioId });
            return exist == 1;
        }

        public async Task<IEnumerable<CountType>> Get(int usuarioId)
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryAsync<CountType>(@"SELECT Id,Nombre,UsuarioId,Orden FROM TiposCuentas 
                                                    WHERE UsuarioId=@UsuarioId", new { usuarioId });
        }
        public async Task Update(CountType countType)
        {
            using var conn = new SqlConnection(connectionString);

            await conn.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre = @Nombre 
                                    WHERE Id= @Id", countType);
        }

        public async Task<CountType> GetCountTypeById(int id, int usuarioId)
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryFirstOrDefaultAsync<CountType>(@"SELECT Nombre, UsuarioId, Orden FROM TiposCuentas 
                                                                  WHERE Id=@Id AND UsuarioId=@UsuarioId",
                                                                  new { id, usuarioId }) ?? new CountType();
        }

        public async Task Delete(int id)
        {
            using var conn = new SqlConnection(connectionString);
            await conn.ExecuteAsync(@"DELETE TiposCuentas WHERE Id=@Id", new {id});
        }
    }
}
