using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PastryOrderRepository : Repository<PastryOrder>, IPastryOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public PastryOrderRepository(ApplicationDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<PastryOrder>> GetAllPastryOrdersAsync(Expression<Func<PastryOrder, bool>> filter = null)
        {
            IQueryable<PastryOrder> query = _db.PastryOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<PastryOrder> GetPastryOrderAsync(Expression<Func<PastryOrder, bool>> filter = null)
        {
            IQueryable<PastryOrder> query = _db.PastryOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task CreatePastryOrderAsync(PastryOrder entity)
        {
            await _db.PastryOrders.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<PastryOrder> UpdatePastryOrderAsync(PastryOrder entity)
        {
            _db.PastryOrders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeletePastryOrderAsync(PastryOrder entity)
        {
            _db.PastryOrders.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
