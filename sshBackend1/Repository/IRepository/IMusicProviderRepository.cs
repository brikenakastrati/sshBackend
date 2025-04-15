
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IMusicProviderRepository : IRepository<MusicProvider>
    {
        Task<IEnumerable<MusicProvider>> GetAllMusicProvidersAsync(Expression<Func<MusicProvider, bool>> filter = null);
        Task<MusicProvider> GetMusicProviderAsync(Expression<Func<MusicProvider, bool>> filter = null);
        Task CreateMusicProviderAsync(MusicProvider entity);
        Task<MusicProvider> UpdateMusicProviderAsync(MusicProvider entity);
        Task DeleteMusicProviderAsync(MusicProvider entity);
        Task SaveAsync();
    }
}
