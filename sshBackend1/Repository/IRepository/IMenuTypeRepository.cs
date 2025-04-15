
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IMenuTypeRepository : IRepository<MenuType>
    {
        Task<IEnumerable<MenuType>> GetAllMenuTypesAsync(Expression<Func<MenuType, bool>> filter = null);
        Task<MenuType> GetMenuTypeAsync(Expression<Func<MenuType, bool>> filter = null);
        Task CreateMenuTypeAsync(MenuType entity);
        Task<MenuType> UpdateMenuTypeAsync(MenuType entity);
        Task DeleteMenuTypeAsync(MenuType entity);
        Task SaveAsync();
    }
}
