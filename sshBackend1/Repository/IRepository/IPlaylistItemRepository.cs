using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPlaylistItemRepository : IRepository<PlaylistItem>
    {
        Task<IEnumerable<PlaylistItem>> GetAllPlaylistItemsAsync(Expression<Func<PlaylistItem, bool>> filter = null);
        Task<PlaylistItem> GetPlaylistItemAsync(Expression<Func<PlaylistItem, bool>> filter = null);
        Task CreatePlaylistItemAsync(PlaylistItem entity);
        Task<PlaylistItem> UpdatePlaylistItemAsync(PlaylistItem entity);
        Task DeletePlaylistItemAsync(PlaylistItem entity);
        Task SaveAsync();
    }
}
