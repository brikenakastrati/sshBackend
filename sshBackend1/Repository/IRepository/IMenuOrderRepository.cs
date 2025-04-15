
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IMenuOrderRepository : IRepository<MenuOrder>
    {
        Task<IEnumerable<MenuOrder>> GetAllMenuOrdersAsync(Expression<Func<MenuOrder, bool>> filter = null);
        Task<MenuOrder> GetMenuOrderAsync(Expression<Func<MenuOrder, bool>> filter = null);
        Task CreateMenuOrderAsync(MenuOrder entity);
        Task<MenuOrder> UpdateMenuOrderAsync(MenuOrder entity);
        Task DeleteMenuOrderAsync(MenuOrder entity);
        Task SaveAsync();
    }
}
