using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPastryShopRepository :IRepository<PastryShop>
    {
        Task<IEnumerable<PastryShop>> GetAllPastryShopsAsync(Expression<Func<PastryShop, bool>> filter = null);
        Task<PastryShop> GetPastryShopAsync(Expression<Func<PastryShop, bool>> filter = null);
        Task CreatePastryShopAsync(PastryShop entity);
        Task<PastryShop> UpdatePastryShopAsync(PastryShop entity);
        Task DeletePastryShopAsync(PastryShop entity);
        Task SaveAsync();
    }
}
