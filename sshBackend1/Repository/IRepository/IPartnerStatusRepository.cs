using sshBackend1.Models;
using System.Linq.Expressions;

namespace sshBackend1.Repository.IRepository
{
    public interface IPartnerStatusRepository
    {
        Task<IEnumerable<PartnerStatus>> GetAllPartnerStatusesAsync(Expression<Func<PartnerStatus, bool>> filter = null);
        Task<PartnerStatus> GetPartnerStatusAsync(Expression<Func<PartnerStatus, bool>> filter = null);
        Task CreatePartnerStatusAsync(PartnerStatus entity);
        Task<PartnerStatus> UpdatePartnerStatusAsync(PartnerStatus entity);
        Task DeletePartnerStatusAsync(PartnerStatus entity);
        Task SaveAsync();
    }
}
