
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IGuestRepository : IRepository<Guest>
    {
        Task<IEnumerable<Guest>> GetAllGuestsAsync(Expression<Func<Guest, bool>> filter = null);
        Task<Guest> GetGuestAsync(Expression<Func<Guest, bool>> filter = null);
        Task CreateGuestAsync(Guest entity);
        Task<Guest> UpdateGuestAsync(Guest entity);
        Task DeleteGuestAsync(Guest entity);
        Task SaveAsync();
    }
}
