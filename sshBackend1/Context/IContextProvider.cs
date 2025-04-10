namespace sshBackend1.Context
{
    public interface IContextProvider
    {
        string? GetCurrentTenantId();
        string? GetCurrentUserId(); // opsionale nëse të duhet
    }
}
