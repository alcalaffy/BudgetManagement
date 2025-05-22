using BudgetManagement.IServices;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly string connectionString;
        public CategoryRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("StoreConnection");
        }
        public async Task Create(Category category)
        {
            using var conn=new SqlConnection(connectionString);
            var id=await conn.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                                                      Values (@Nombre, @TipoOperacionId, @UsuarioId);
                                                      SELECT SCOPE_IDENTITY();",category);
            category.Id=id;
        }
        public async Task<IEnumerable<Category>>GetCategories(int userId)
        {
            using var conn=new SqlConnection(connectionString);
            return await conn.QueryAsync<Category>(@"SELECT Id,Nombre,TipoOperacionId,UsuarioId
                                                     FROM Categorias WHERE UsuarioId=@userId", new { userId });
        }
        public async Task<Category>GetById(int Id,int userId)
        {
            using var conn = new SqlConnection(connectionString);
            return await conn.QueryFirstOrDefaultAsync<Category>(@"SELECT * FROM Categorias 
                                                                   WHERE Id = @Id AND UsuarioId = @userId", new {Id, userId });
        }
        public async Task Update(Category category)
        {
            using var conn = new SqlConnection(connectionString);
            await conn.ExecuteAsync(@"UPDATE Categorias SET Nombre=@Nombre,TipoOperacionId=@TipoOperacionId 
                                      WHERE Id=@Id AND UsuarioId=@UsuarioId",category);
        }
        public async Task Delete(int id,int userId)
        {
            using var conn= new SqlConnection(connectionString);
            await conn.ExecuteAsync(@"DELETE Categorias WHERE UsuarioId=@userId AND Id=@id", new {id, userId});
        }

    }
}
