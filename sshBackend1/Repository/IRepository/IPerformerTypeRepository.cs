
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPerformerTypeRepository : IRepository<PerformerType>
    {
        Task<IEnumerable<PerformerType>> GetAllPerformerTypesAsync(Expression<Func<PerformerType, bool>> filter = null);
        Task<PerformerType> GetPerformerTypeAsync(Expression<Func<PerformerType, bool>> filter = null);
        Task CreatePerformerTypeAsync(PerformerType entity);
        Task<PerformerType> UpdatePerformerTypeAsync(PerformerType entity);
        Task DeletePerformerTypeAsync(PerformerType entity);
        Task SaveAsync();
    }
}
