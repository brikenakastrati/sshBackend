using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class TableRepository : Repository<Table>, ITableRepository
    {
        private readonly ApplicationDbContext _db;
        public TableRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Table>> GetAllTablesAsync(Expression<Func<Table, bool>> filter = null)
        {
            IQueryable<Table> query = _db.Tables;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Table> GetTableAsync(Expression<Func<Table, bool>> filter = null)
        {
            IQueryable<Table> query = _db.Tables;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateTableAsync(Table entity)
        {
            await _db.Tables.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Table> UpdateTableAsync(Table entity)
        {
            _db.Tables.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteTableAsync(Table entity)
        {
            _db.Tables.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

