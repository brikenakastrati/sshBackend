using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IVenueOrderRepository : IRepository<VenueOrder>
    {
        Task<IEnumerable<VenueOrder>> GetAllVenueOrdersAsync(Expression<Func<VenueOrder, bool>> filter = null);
        Task<VenueOrder> GetVenueOrderAsync(Expression<Func<VenueOrder, bool>> filter = null);
        Task CreateVenueOrderAsync(VenueOrder entity);
        Task<VenueOrder> UpdateVenueOrderAsync(VenueOrder entity);
        Task DeleteVenueOrderAsync(VenueOrder entity);
        Task SaveAsync();
    }
}