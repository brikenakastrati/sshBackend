namespace sshBackend1.Services.IServices
{
    public interface ICacheService
    {
        T GetOrAdd<T>(string key, Func<T> acquire, TimeSpan duration);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> acquire, TimeSpan duration);
    }
}
