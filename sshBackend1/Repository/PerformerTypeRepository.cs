using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PerformerTypeRepository : Repository<PerformerType>, IPerformerTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public PerformerTypeRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<PerformerType>> GetAllPerformerTypesAsync(Expression<Func<PerformerType, bool>> filter = null)
        {
            IQueryable<PerformerType> query = _db.PerformerTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<PerformerType> GetPerformerTypeAsync(Expression<Func<PerformerType, bool>> filter = null)
        {
            IQueryable<PerformerType> query = _db.PerformerTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreatePerformerTypeAsync(PerformerType entity)
        {
            await _db.PerformerTypes.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<PerformerType> UpdatePerformerTypeAsync(PerformerType entity)
        {
            _db.PerformerTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeletePerformerTypeAsync(PerformerType entity)
        {
            _db.PerformerTypes.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

