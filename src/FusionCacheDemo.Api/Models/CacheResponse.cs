namespace FusionCacheDemo.Api.Models;

public class CacheResponse<T>
{
    public T Data { get; set; } = default!;
    public double ResponseTime { get; set; }
    public string CacheType { get; set; } = string.Empty;
}