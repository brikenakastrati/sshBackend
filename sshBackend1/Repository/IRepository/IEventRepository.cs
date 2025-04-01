using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IEventRepository : IRepository<Event>
    {
        // Method to get all events
        Task<IEnumerable<Event>> GetAllEventsAsync(Expression<Func<Event, bool>> filter = null);
        // Method to get a specific event by ID
        Task<Event> GetEventAsync(Expression<Func<Event, bool>> filter = null);  
         // Method to create a new event
        Task CreateEventAsync(Event entity);
        // Method to update an existing event
        Task<Event> UpdateEventAsync(Event entity);
        // Method to delete an event by ID
        Task DeleteEventAsync(Event entity);
        // Method to save changes to the database
        Task SaveAsync();
    }
}
