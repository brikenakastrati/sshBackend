
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync(Expression<Func<Restaurant, bool>> filter = null);
        Task<Restaurant> GetRestaurantAsync(Expression<Func<Restaurant, bool>> filter = null);
        Task CreateRestaurantAsync(Restaurant entity);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant entity);
        Task DeleteRestaurantAsync(Restaurant entity);
        Task SaveAsync();
    }
}
