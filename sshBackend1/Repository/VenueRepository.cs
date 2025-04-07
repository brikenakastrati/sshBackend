using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class VenueRepository : Repository<Venue>, IVenueRepository
    {
        private readonly ApplicationDbContext _db;
        public VenueRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Venue>> GetAllVenuesAsync(Expression<Func<Venue, bool>> filter = null)
        {
            IQueryable<Venue> query = _db.Venues;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Venue> GetVenueAsync(Expression<Func<Venue, bool>> filter = null)
        {
            IQueryable<Venue> query = _db.Venues;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateVenueAsync(Venue entity)
        {
            await _db.Venues.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Venue> UpdateVenueAsync(Venue entity)
        {
            _db.Venues.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteVenueAsync(Venue entity)
        {
            _db.Venues.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

