using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;
using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PastryShopRepository : Repository<PastryShop>, IPastryShopRepository
    {
        private readonly ApplicationDbContext _db;
        public PastryShopRepository(ApplicationDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<PastryShop>> GetAllPastryShopsAsync(Expression<Func<PastryShop, bool>> filter = null)
        {
            IQueryable<PastryShop> query = _db.PastryShops;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<PastryShop> GetPastryShopAsync(Expression<Func<PastryShop, bool>> filter = null)
        {
            IQueryable<PastryShop> query = _db.PastryShops;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task CreatePastryShopAsync(PastryShop entity)
        {
            await _db.PastryShops.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<PastryShop> UpdatePastryShopAsync(PastryShop entity)
        {
            _db.PastryShops.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeletePastryShopAsync(PastryShop entity)
        {
            _db.PastryShops.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
