using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class FlowerArrangementOrderRepository : Repository<FlowerArrangementOrder>, IFlowerArrangementOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public FlowerArrangementOrderRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<FlowerArrangementOrder>> GetAllFlowerArrangementOrdersAsync(Expression<Func<FlowerArrangementOrder, bool>> filter = null)
        {
            IQueryable<FlowerArrangementOrder> query = _db.FlowerArrangementOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<FlowerArrangementOrder> GetFlowerArrangementOrderAsync(Expression<Func<FlowerArrangementOrder, bool>> filter = null)
        {
            IQueryable<FlowerArrangementOrder> query = _db.FlowerArrangementOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateFlowerArrangementOrderAsync(FlowerArrangementOrder entity)
        {
            await _db.FlowerArrangementOrders.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<FlowerArrangementOrder> UpdateFlowerArrangementOrderAsync(FlowerArrangementOrder entity)
        {
            _db.FlowerArrangementOrders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteFlowerArrangementOrderAsync(FlowerArrangementOrder entity)
        {
            _db.FlowerArrangementOrders.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

