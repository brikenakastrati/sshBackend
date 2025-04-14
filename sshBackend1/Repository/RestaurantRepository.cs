using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        private readonly ApplicationDbContext _db;
        public RestaurantRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync(Expression<Func<Restaurant, bool>> filter = null)
        {
            IQueryable<Restaurant> query = _db.Restaurants;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<Restaurant> GetRestaurantAsync(Expression<Func<Restaurant, bool>> filter = null)
        {
            IQueryable<Restaurant> query = _db.Restaurants;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateRestaurantAsync(Restaurant entity)
        {
            await _db.Restaurants.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<Restaurant> UpdateRestaurantAsync(Restaurant entity)
        {
            _db.Restaurants.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteRestaurantAsync(Restaurant entity)
        {
            _db.Restaurants.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

