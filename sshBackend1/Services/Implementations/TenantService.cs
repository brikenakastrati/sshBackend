using sshBackend1.Services.Interfaces;

namespace sshBackend1.Services.Implementations
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetTenantId()
        {
            return _httpContextAccessor.HttpContext?.Request.Headers["Tenant-Id"].ToString() ?? string.Empty;
        }
    }
}
