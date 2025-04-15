using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class MusicProviderRepository : Repository<MusicProvider>, IMusicProviderRepository
    {
        private readonly ApplicationDbContext _db;
        public MusicProviderRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<MusicProvider>> GetAllMusicProvidersAsync(Expression<Func<MusicProvider, bool>> filter = null)
        {
            IQueryable<MusicProvider> query = _db.MusicProviders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<MusicProvider> GetMusicProviderAsync(Expression<Func<MusicProvider, bool>> filter = null)
        {
            IQueryable<MusicProvider> query = _db.MusicProviders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateMusicProviderAsync(MusicProvider entity)
        {
            await _db.MusicProviders.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<MusicProvider> UpdateMusicProviderAsync(MusicProvider entity)
        {
            _db.MusicProviders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteMusicProviderAsync(MusicProvider entity)
        {
            _db.MusicProviders.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

