using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PastryRepository : Repository<Pastry>, IPastryRepository
    {
        private readonly ApplicationDbContext _db;
        public PastryRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Pastry>> GetAllPastriesAsync(Expression<Func<Pastry, bool>> filter = null)
        {
            IQueryable<Pastry> query = _db.Pastries;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Pastry> GetPastryAsync(Expression<Func<Pastry, bool>> filter = null)
        {
            IQueryable<Pastry> query = _db.Pastries;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreatePastryAsync(Pastry entity)
        {
            await _db.Pastries.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Pastry> UpdatePastryAsync(Pastry entity)
        {
            _db.Pastries.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeletePastryAsync(Pastry entity)
        {
            _db.Pastries.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

