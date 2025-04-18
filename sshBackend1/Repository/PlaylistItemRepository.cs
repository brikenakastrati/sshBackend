using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PlaylistItemRepository : Repository<PlaylistItem>, IPlaylistItemRepository
    {
        private readonly ApplicationDbContext _db;
        public PlaylistItemRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<PlaylistItem>> GetAllPlaylistItemsAsync(Expression<Func<PlaylistItem, bool>> filter = null)
        {
            IQueryable<PlaylistItem> query = _db.PlaylistItems;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<PlaylistItem> GetPlaylistItemAsync(Expression<Func<PlaylistItem, bool>> filter = null)
        {
            IQueryable<PlaylistItem> query = _db.PlaylistItems;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreatePlaylistItemAsync(PlaylistItem entity)
        {
            await _db.PlaylistItems.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<PlaylistItem> UpdatePlaylistItemAsync(PlaylistItem entity)
        {
            _db.PlaylistItems.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeletePlaylistItemAsync(PlaylistItem entity)
        {
            _db.PlaylistItems.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

