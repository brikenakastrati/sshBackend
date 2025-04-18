using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;

using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class PartnerStatusRepository : Repository<PartnerStatus>, IPartnerStatusRepository
    {
        private readonly ApplicationDbContext _db;
        public PartnerStatusRepository(ApplicationDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<PartnerStatus>> GetAllPartnerStatusesAsync(Expression<Func<PartnerStatus, bool>> filter = null)
        {
            IQueryable<PartnerStatus> query = _db.PartnerStatuses;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<PartnerStatus> GetPartnerStatusAsync(Expression<Func<PartnerStatus, bool>> filter = null)
        {
            IQueryable<PartnerStatus> query = _db.PartnerStatuses;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task CreatePartnerStatusAsync(PartnerStatus entity)
        {
            await _db.PartnerStatuses.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<PartnerStatus> UpdatePartnerStatusAsync(PartnerStatus entity)
        {
            _db.PartnerStatuses.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeletePartnerStatusAsync(PartnerStatus entity)
        {
            _db.PartnerStatuses.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
