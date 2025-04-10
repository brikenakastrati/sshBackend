using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace sshBackend1.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Merr tenantId nga header (ose nga domaini, nëse e përdor atë)
            var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }

            await _next(context);
        }
    }
}
