using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPastryOrderRepository: IRepository<PastryOrder>
    {
        Task<IEnumerable<PastryOrder>> GetAllPastryOrdersAsync(Expression<Func<PastryOrder, bool>> filter = null);
        Task<PastryOrder> GetPastryOrderAsync(Expression<Func<PastryOrder, bool>> filter = null);
        Task CreatePastryOrderAsync(PastryOrder entity);
        Task<PastryOrder> UpdatePastryOrderAsync(PastryOrder entity);
        Task DeletePastryOrderAsync(PastryOrder entity);
        Task SaveAsync();



    }
}
