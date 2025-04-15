using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _db;
        public MenuRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Menu>> GetAllMenusAsync(Expression<Func<Menu, bool>> filter = null)
        {
            IQueryable<Menu> query = _db.Menu;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Menu> GetMenuAsync(Expression<Func<Menu, bool>> filter = null)
        {
            IQueryable<Menu> query = _db.Menu;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateMenuAsync(Menu entity)
        {
            await _db.Menu.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Menu> UpdateMenuAsync(Menu entity)
        {
            _db.Menu.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteMenuAsync(Menu entity)
        {
            _db.Menu.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

