using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class MenuOrderRepository : Repository<MenuOrder>, IMenuOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public MenuOrderRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<MenuOrder>> GetAllMenuOrdersAsync(Expression<Func<MenuOrder, bool>> filter = null)
        {
            IQueryable<MenuOrder> query = _db.MenuOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<MenuOrder> GetMenuOrderAsync(Expression<Func<MenuOrder, bool>> filter = null)
        {
            IQueryable<MenuOrder> query = _db.MenuOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateMenuOrderAsync(MenuOrder entity)
        {
            await _db.MenuOrders.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<MenuOrder> UpdateMenuOrderAsync(MenuOrder entity)
        {
            _db.MenuOrders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteMenuOrderAsync(MenuOrder entity)
        {
            _db.MenuOrders.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

