
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IFlowerArrangementOrderRepository : IRepository<FlowerArrangementOrder>
    {
        Task<IEnumerable<FlowerArrangementOrder>> GetAllFlowerArrangementOrdersAsync(Expression<Func<FlowerArrangementOrder, bool>> filter = null);
        Task<FlowerArrangementOrder> GetFlowerArrangementOrderAsync(Expression<Func<FlowerArrangementOrder, bool>> filter = null);
        Task CreateFlowerArrangementOrderAsync(FlowerArrangementOrder entity);
        Task<FlowerArrangementOrder> UpdateFlowerArrangementOrderAsync(FlowerArrangementOrder entity);
        Task DeleteFlowerArrangementOrderAsync(FlowerArrangementOrder entity);
        Task SaveAsync();
    }
}
