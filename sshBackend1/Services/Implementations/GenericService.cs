using sshBackend1.Data;
using sshBackend1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using sshBackend1.Context;

namespace sshBackend1.Services.Implementations
{
    public class GenericService<T> : IGenericService<T> where T : class, ITenantEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly IContextProvider _contextProvider;

        public GenericService(ApplicationDbContext context, IContextProvider contextProvider)
        {
            _context = context;
            _contextProvider = contextProvider;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var tenantId = _contextProvider.GetCurrentTenantId();
            return await _context.Set<T>()
                .Where(x => x.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            // Ensure entity implements ITenantEntity and set the TenantId
            if (entity is ITenantEntity tenantEntity)
            {
                tenantEntity.TenantId = _contextProvider.GetCurrentTenantId();
            }
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
