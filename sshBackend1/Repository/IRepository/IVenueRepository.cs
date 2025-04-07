
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IVenueRepository : IRepository<Venue>
    {
        Task<IEnumerable<Venue>> GetAllVenuesAsync(Expression<Func<Venue, bool>> filter = null);
        Task<Venue> GetVenueAsync(Expression<Func<Venue, bool>> filter = null);
        Task CreateVenueAsync(Venue entity);
        Task<Venue> UpdateVenueAsync(Venue entity);
        Task DeleteVenueAsync(Venue entity);
        Task SaveAsync();
    }
}
