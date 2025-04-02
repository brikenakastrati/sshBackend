using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;
using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class FloristRepository : Repository<Florist>, IFloristRepository
    {
        private readonly ApplicationDbContext _db;
        public FloristRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Florist>> GetAllFloristsAsync(Expression<Func<Florist, bool>> filter = null)
        {
            IQueryable<Florist> query = _db.Florists;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Florist> GetFloristAsync(Expression<Func<Florist, bool>> filter = null)
        {
            IQueryable<Florist> query = _db.Florists;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateFloristAsync(Florist entity)
        {
            await _db.Florists.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Florist> UpdateFloristAsync(Florist entity)
        {
            _db.Florists.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteFloristAsync(Florist entity)
        {
            _db.Florists.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
