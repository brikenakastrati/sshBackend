using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class MenuTypeRepository : Repository<MenuType>, IMenuTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public MenuTypeRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<MenuType>> GetAllMenuTypesAsync(Expression<Func<MenuType, bool>> filter = null)
        {
            IQueryable<MenuType> query = _db.MenuTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<MenuType> GetMenuTypeAsync(Expression<Func<MenuType, bool>> filter = null)
        {
            IQueryable<MenuType> query = _db.MenuTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateMenuTypeAsync(MenuType entity)
        {
            await _db.MenuTypes.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<MenuType> UpdateMenuTypeAsync(MenuType entity)
        {
            _db.MenuTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteMenuTypeAsync(MenuType entity)
        {
            _db.MenuTypes.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

