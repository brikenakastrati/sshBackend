
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IRestaurantStatusRepository : IRepository<RestaurantStatus>
    {
        Task<IEnumerable<RestaurantStatus>> GetAllRestaurantStatusesAsync(Expression<Func<RestaurantStatus, bool>> filter = null);
        Task<RestaurantStatus> GetRestaurantStatusAsync(Expression<Func<RestaurantStatus, bool>> filter = null);
        Task CreateRestaurantStatusAsync(RestaurantStatus entity);
        Task<RestaurantStatus> UpdateRestaurantStatusAsync(RestaurantStatus entity);
        Task DeleteRestaurantStatusAsync(RestaurantStatus entity);
        Task SaveAsync();
    }
}
