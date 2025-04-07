using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class FlowerArrangementTypeRepository : Repository<FlowerArrangementType>, IFlowerArrangementTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public FlowerArrangementTypeRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<FlowerArrangementType>> GetAllFlowerArrangementTypesAsync(Expression<Func<FlowerArrangementType, bool>> filter = null)
        {
            IQueryable<FlowerArrangementType> query = _db.FlowerArrangementTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<FlowerArrangementType> GetFlowerArrangementTypeAsync(Expression<Func<FlowerArrangementType, bool>> filter = null)
        {
            IQueryable<FlowerArrangementType> query = _db.FlowerArrangementTypes;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateFlowerArrangementTypeAsync(FlowerArrangementType entity)
        {
            await _db.FlowerArrangementTypes.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<FlowerArrangementType> UpdateFlowerArrangementTypeAsync(FlowerArrangementType entity)
        {
            _db.FlowerArrangementTypes.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteFlowerArrangementTypesAsync(FlowerArrangementType entity)
        {
            _db.FlowerArrangementTypes.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

