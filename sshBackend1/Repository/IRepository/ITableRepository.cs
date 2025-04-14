
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface ITableRepository : IRepository<Table>
    {
        Task<IEnumerable<Table>> GetAllTablesAsync(Expression<Func<Table, bool>> filter = null);
        Task<Table> GetTableAsync(Expression<Func<Table, bool>> filter = null);
        Task CreateTableAsync(Table entity);
        Task<Table> UpdateTableAsync(Table entity);
        Task DeleteTableAsync(Table entity);
        Task SaveAsync();
    }
}
