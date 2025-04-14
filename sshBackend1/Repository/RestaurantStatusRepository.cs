using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class RestaurantStatusRepository : Repository<RestaurantStatus>, IRestaurantStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public RestaurantStatusRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<RestaurantStatus>> GetAllRestaurantStatusesAsync(Expression<Func<RestaurantStatus, bool>> filter = null)
        {
            IQueryable<RestaurantStatus> query = _db.RestaurantStatuses;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<RestaurantStatus> GetRestaurantStatusAsync(Expression<Func<RestaurantStatus, bool>> filter = null)
        {
            IQueryable<RestaurantStatus> query = _db.RestaurantStatuses;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateRestaurantStatusAsync(RestaurantStatus entity)
        {
            await _db.RestaurantStatuses.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<RestaurantStatus> UpdateRestaurantStatusAsync(RestaurantStatus entity)
        {
            _db.RestaurantStatuses.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteRestaurantStatusAsync(RestaurantStatus entity)
        {
            _db.RestaurantStatuses.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

