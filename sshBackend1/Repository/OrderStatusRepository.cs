using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderStatusRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<OrderStatus>> GetAllOrderStatusesAsync(Expression<Func<OrderStatus, bool>> filter = null)
        {
            IQueryable<OrderStatus> query = _db.OrderStatuses;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<OrderStatus> GetOrderStatusAsync(Expression<Func<OrderStatus, bool>> filter = null)
        {
            IQueryable<OrderStatus> query = _db.OrderStatuses;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateOrderStatusAsync(OrderStatus entity)
        {
            await _db.OrderStatuses.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<OrderStatus> UpdateOrderStatusAsync(OrderStatus entity)
        {
            _db.OrderStatuses.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteOrderStatusAsync(OrderStatus entity)
        {
            _db.OrderStatuses.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

