using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPastryTypeRepository : IRepository<PastryType>
    {
        Task<IEnumerable<PastryType>> GetAllPastryTypesAsync(Expression<Func<PastryType, bool>> filter = null);
        Task<PastryType> GetPastryTypeAsync(Expression<Func<PastryType, bool>> filter = null);
        Task CreatePastryTypeAsync(PastryType entity);
        Task<PastryType> UpdatePastryTypeAsync(PastryType entity);
        Task DeletePastryTypeAsync(PastryType entity);
        Task SaveAsync();
    }
}
