using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IFloristRepository : IRepository<Florist>
    {

        Task<IEnumerable<Florist>> GetAllFloristsAsync(Expression<Func<Florist, bool>> filter = null);

        Task<Florist> GetFloristAsync(Expression<Func<Florist, bool>> filter = null);

        Task CreateFloristAsync(Florist entity);

        Task<Florist> UpdateFloristAsync(Florist entity);

        Task DeleteFloristAsync(Florist entity);

        Task SaveAsync();
    }
}