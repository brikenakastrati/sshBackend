using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class VenueProviderRepository : Repository<VenueProvider>, IVenueProviderRepository
    {
        private readonly ApplicationDbContext _db;
        public VenueProviderRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<VenueProvider>> GetAllVenueProviderAsync(Expression<Func<VenueProvider, bool>> filter = null)
        {
            IQueryable<VenueProvider> query = _db.VenueProviders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<VenueProvider> GetVenueProviderAsync(Expression<Func<VenueProvider, bool>> filter = null)
        {
            IQueryable<VenueProvider> query = _db.VenueProviders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateVenueProviderAsync(VenueProvider entity)
        {
            await _db.VenueProviders.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<VenueProvider> UpdateVenueProviderAsync(VenueProvider entity)
        {
            _db.VenueProviders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteVenueProviderAsync(VenueProvider entity)
        {
            _db.VenueProviders.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

