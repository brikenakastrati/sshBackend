using sshBackend1.Repository;
using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IFlowerArrangementTypeRepository : IRepository<FlowerArrangementType>
    {
        Task<IEnumerable<FlowerArrangementType>> GetAllFlowerArrangementTypesAsync(Expression<Func<FlowerArrangementType, bool>> filter = null);
        Task<FlowerArrangementType> GetFlowerArrangementTypeAsync(Expression<Func<FlowerArrangementType, bool>> filter = null);
        Task CreateFlowerArrangementTypeAsync(FlowerArrangementType entity);
        Task<FlowerArrangementType> UpdateFlowerArrangementTypeAsync(FlowerArrangementType entity);
        Task DeleteFlowerArrangementTypesAsync(FlowerArrangementType entity);
        Task SaveAsync();

    }
}
