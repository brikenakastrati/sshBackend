using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;
using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PastryTypeRepository : Repository<PastryType>, IPastryTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public PastryTypeRepository(ApplicationDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<PastryType>> GetAllPastryTypesAsync(Expression<Func<PastryType, bool>> filter = null)
        {
            IQueryable<PastryType> query = _db.PastryTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<PastryType> GetPastryTypeAsync(Expression<Func<PastryType, bool>> filter = null)
        {
            IQueryable<PastryType> query = _db.PastryTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task CreatePastryTypeAsync(PastryType entity)
        {
            await _db.PastryTypes.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<PastryType> UpdatePastryTypeAsync(PastryType entity)
        {
            _db.PastryTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeletePastryTypeAsync(PastryType entity)
        {
            _db.PastryTypes.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
