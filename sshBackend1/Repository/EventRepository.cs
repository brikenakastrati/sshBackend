using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _db;
        public EventRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Event>> GetAllEventsAsync(Expression<Func<Event, bool>> filter = null)
        {
            IQueryable<Event> query = _db.Events;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Event> GetEventAsync(Expression<Func<Event, bool>> filter = null)
        {
            IQueryable<Event> query = _db.Events;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateEventAsync(Event entity)
        {
            await _db.Events.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Event> UpdateEventAsync(Event entity)
        {
            _db.Events.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteEventAsync(Event entity)
        {
            _db.Events.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

       
    }
}

