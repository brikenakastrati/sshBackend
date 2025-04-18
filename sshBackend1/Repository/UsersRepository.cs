using Microsoft.EntityFrameworkCore;
using sshBackend1.Data;
using sshBackend1.Models;
using sshBackend1.Repository.IRepository;
using System.Linq.Expressions;

namespace sshBackend1.Repository
{
    public class UserRepository : Repository<Users>, IUsersRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db) => _db = db;

        public async Task<IEnumerable<Users>> GetAllUsersAsync(Expression<Func<Users, bool>> filter = null)
        {
            IQueryable<Users> query = _db.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<Users> GetUsersAsync(Expression<Func<Users, bool>> filter = null)
        {
            IQueryable<Users> query = _db.Users;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateUsersAsync(Users entity)
        {
            await _db.Users.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<Users> UpdateUsersAsync(Users entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteUsersAsync(Users entity)
        {
            _db.Users.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
