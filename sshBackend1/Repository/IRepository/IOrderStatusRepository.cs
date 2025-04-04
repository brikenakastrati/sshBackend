
using Nest;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IOrderStatusRepository : IRepository<OrderStatus>
    {
        Task<IEnumerable<OrderStatus>> GetAllOrderStatusesAsync(Expression<Func<OrderStatus, bool>> filter = null);
        Task<OrderStatus> GetOrderStatusAsync(Expression<Func<OrderStatus, bool>> filter = null);
        Task CreateOrderStatusAsync(OrderStatus entity);
        Task<OrderStatus> UpdateOrderStatusAsync(OrderStatus entity);
        Task DeleteOrderStatusAsync(OrderStatus entity);
        Task SaveAsync();
    }
}
