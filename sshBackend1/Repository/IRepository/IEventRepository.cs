
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(Expression<Func<Event, bool>> filter = null);
        Task<Event> GetEventAsync(Expression<Func<Event, bool>> filter = null);
        Task CreateEventAsync(Event entity);
        Task<Event> UpdateEventAsync(Event entity);
        Task DeleteEventAsync(Event entity);
        Task SaveAsync();
    }
}
