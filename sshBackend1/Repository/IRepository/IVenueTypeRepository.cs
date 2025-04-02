
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IVenueTypeRepository : IRepository<VenueType>
    {
        Task<IEnumerable<VenueType>> GetAllVenueTypesAsync(Expression<Func<VenueType, bool>> filter = null);
        Task<VenueType> GetVenueTypeAsync(Expression<Func<VenueType, bool>> filter = null);
        
        Task CreateVenueTypeAsync(VenueType entity);
        Task<VenueType> UpdateVenueTypeAsync(VenueType entity);
        Task DeleteVenueTypeAsync(VenueType entity);
        Task SaveAsync();
    }
}
