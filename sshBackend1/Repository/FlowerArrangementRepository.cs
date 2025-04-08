using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class FlowerArrangementRepository : Repository<FlowerArrangement>, IFlowerArrangementRepository
    {
        private readonly ApplicationDbContext _db;
        public FlowerArrangementRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<FlowerArrangement>> GetAllFlowerArrangementsAsync(Expression<Func<FlowerArrangement, bool>> filter = null)
        {
            IQueryable<FlowerArrangement> query = _db.FlowerArrangements;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<FlowerArrangement> GetFlowerArrangementAsync(Expression<Func<FlowerArrangement, bool>> filter = null)
        {
            IQueryable<FlowerArrangement> query = _db.FlowerArrangements;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateFlowerArrangementAsync(FlowerArrangement entity)
        {
            await _db.FlowerArrangements.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<FlowerArrangement> UpdateFlowerArrangementAsync(FlowerArrangement entity)
        {
            _db.FlowerArrangements.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteFlowerArrangementAsync(FlowerArrangement entity)
        {
            _db.FlowerArrangements.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

