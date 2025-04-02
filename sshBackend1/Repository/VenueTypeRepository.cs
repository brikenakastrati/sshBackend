using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class VenueTypeRepository : Repository<VenueType>, IVenueTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public VenueTypeRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<VenueType>> GetAllVenueTypesAsync(Expression<Func<VenueType, bool>> filter = null)
        {
            IQueryable<VenueType> query = _db.VenueTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<VenueType> GetVenueTypeAsync(Expression<Func<VenueType, bool>> filter = null)
        {
            IQueryable<VenueType> query = _db.VenueTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateVenueTypeAsync(VenueType entity)
        {
            await _db.VenueTypes.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<VenueType> UpdateVenueTypeAsync(VenueType entity)
        {
            _db.VenueTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteVenueTypeAsync(VenueType entity)
        {
            _db.VenueTypes.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

