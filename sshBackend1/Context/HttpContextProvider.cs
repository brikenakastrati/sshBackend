using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace sshBackend1.Context
{
    public class HttpContextProvider : IContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentTenantId()
        {
            return _httpContextAccessor.HttpContext?.Items["TenantId"]?.ToString();
        }

        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
