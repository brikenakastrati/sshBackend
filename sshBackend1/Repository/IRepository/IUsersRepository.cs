
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IUsersRepository : IRepository<Users>
    {
        Task<IEnumerable<Users>> GetAllUsersAsync(Expression<Func<Users, bool>> filter = null);
        Task<Users> GetUsersAsync(Expression<Func<Users, bool>> filter = null);
        Task CreateUsersAsync(Users entity);
        Task<Users> UpdateUsersAsync(Users entity);
        Task DeleteUsersAsync(Users entity);
        Task SaveAsync();
    }
}
