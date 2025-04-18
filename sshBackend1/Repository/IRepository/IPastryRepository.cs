
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPastryRepository : IRepository<Pastry>
    {
        Task<IEnumerable<Pastry>> GetAllPastriesAsync(Expression<Func<Pastry, bool>> filter = null);
        Task<Pastry> GetPastryAsync(Expression<Func<Pastry, bool>> filter = null);
        Task CreatePastryAsync(Pastry entity);
        Task<Pastry> UpdatePastryAsync(Pastry entity);
        Task DeletePastryAsync(Pastry entity);
        Task SaveAsync();
    }
}
