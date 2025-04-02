
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IVenueProviderRepository : IRepository<VenueProvider>
    {
        // Method to get all events
        Task<IEnumerable<VenueProvider>> GetAllVenueProviderAsync(Expression<Func<VenueProvider, bool>> filter = null);
        // Method to get a specific event by ID
        Task<VenueProvider> GetVenueProviderAsync(Expression<Func<VenueProvider, bool>> filter = null);
        // Method to create a new event
        Task CreateVenueProviderAsync(VenueProvider entity);
        // Method to update an existing event
        Task<VenueProvider> UpdateVenueProviderAsync(VenueProvider entity);
        // Method to delete an event by ID
        Task DeleteVenueProviderAsync(VenueProvider entity);
        // Method to save changes to the database
        Task SaveAsync();
    }
}
