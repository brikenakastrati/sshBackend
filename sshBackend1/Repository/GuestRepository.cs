using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class GuestRepository : Repository<Guest>, IGuestRepository
    {
        private readonly ApplicationDbContext _db;
        public GuestRepository(ApplicationDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<Guest>> GetAllGuestsAsync(Expression<Func<Guest, bool>> filter = null)
        {
            IQueryable<Guest> query = _db.Guests;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Guest> GetGuestAsync(Expression<Func<Guest, bool>> filter = null)
        {
            IQueryable<Guest> query = _db.Guests;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateGuestAsync(Guest entity)
        {
            await _db.Guests.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Guest> UpdateGuestAsync(Guest entity)
        {
            _db.Guests.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteGuestAsync(Guest entity)
        {
            _db.Guests.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
