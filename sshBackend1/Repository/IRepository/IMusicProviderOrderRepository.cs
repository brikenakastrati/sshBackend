
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IMusicProviderOrderRepository : IRepository<MusicProviderOrder>
    {
        Task<IEnumerable<MusicProviderOrder>> GetAllMusicProviderOrdersAsync(Expression<Func<MusicProviderOrder, bool>> filter = null);
        Task<MusicProviderOrder> GetMusicProviderOrderAsync(Expression<Func<MusicProviderOrder, bool>> filter = null);
        Task CreateMusicProviderOrderAsync(MusicProviderOrder entity);
        Task<MusicProviderOrder> UpdateMusicProviderOrderAsync(MusicProviderOrder entity);
        Task DeleteMusicProviderOrderAsync(MusicProviderOrder entity);
        Task SaveAsync();
    }
}
