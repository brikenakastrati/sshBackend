using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class VenueOrderRepository : Repository<VenueOrder>, IVenueOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public VenueOrderRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<VenueOrder>> GetAllVenueOrdersAsync(Expression<Func<VenueOrder, bool>> filter = null)
        {
            IQueryable<VenueOrder> query = _db.VenueOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<VenueOrder> GetVenueOrderAsync(Expression<Func<VenueOrder, bool>> filter = null)
        {
            IQueryable<VenueOrder> query = _db.VenueOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateVenueOrderAsync(VenueOrder entity)
        {
            await _db.VenueOrders.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<VenueOrder> UpdateVenueOrderAsync(VenueOrder entity)
        {
            _db.VenueOrders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteVenueOrderAsync(VenueOrder entity)
        {
            _db.VenueOrders.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

