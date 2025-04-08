
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IFlowerArrangementRepository : IRepository<FlowerArrangement>
    {
        Task<IEnumerable<FlowerArrangement>> GetAllFlowerArrangementsAsync(Expression<Func<FlowerArrangement, bool>> filter = null);
        Task<FlowerArrangement> GetFlowerArrangementAsync(Expression<Func<FlowerArrangement, bool>> filter = null);
        Task CreateFlowerArrangementAsync(FlowerArrangement entity);
        Task<FlowerArrangement> UpdateFlowerArrangementAsync(FlowerArrangement entity);
        Task DeleteFlowerArrangementAsync(FlowerArrangement entity);
        Task SaveAsync();
    }
}
