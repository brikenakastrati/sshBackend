
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IMenuRepository : IRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetAllMenusAsync(Expression<Func<Menu, bool>> filter = null);
        Task<Menu> GetMenuAsync(Expression<Func<Menu, bool>> filter = null);
        Task CreateMenuAsync(Menu entity);
        Task<Menu> UpdateMenuAsync(Menu entity);
        Task DeleteMenuAsync(Menu entity);
        Task SaveAsync();
    }
}
