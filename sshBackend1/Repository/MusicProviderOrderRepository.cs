using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class MusicProviderOrderRepository : Repository<MusicProviderOrder>, IMusicProviderOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public MusicProviderOrderRepository(ApplicationDbContext db) : base(db) => _db = db;
        public async Task<IEnumerable<MusicProviderOrder>> GetAllMusicProviderOrdersAsync(Expression<Func<MusicProviderOrder, bool>> filter = null)
        {
            IQueryable<MusicProviderOrder> query = _db.MusicProviderOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public async Task<MusicProviderOrder> GetMusicProviderOrderAsync(Expression<Func<MusicProviderOrder, bool>> filter = null)
        {
            IQueryable<MusicProviderOrder> query = _db.MusicProviderOrders;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task CreateMusicProviderOrderAsync(MusicProviderOrder entity)
        {
            await _db.MusicProviderOrders.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<MusicProviderOrder> UpdateMusicProviderOrderAsync(MusicProviderOrder entity)
        {
            _db.MusicProviderOrders.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteMusicProviderOrderAsync(MusicProviderOrder entity)
        {
            _db.MusicProviderOrders.Remove(entity);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}

