namespace FusionCacheDemo.Api.Models;

// Request DTO
public class BenchmarkRequest
{
    public string Url { get; set; }
    public int Rpm { get; set; }
    public int Time { get; set; } // Time in seconds
}

// Response DTO
public class BenchmarkReport
{
    public int TotalRequests { get; set; }
    public int SuccessfulRequests { get; set; }
    public double AverageResponseTime { get; set; }
    public long MinResponseTime { get; set; }
    public long MaxResponseTime { get; set; }
}