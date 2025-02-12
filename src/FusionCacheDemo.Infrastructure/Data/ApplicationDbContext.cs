using FusionCacheDemo.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FusionCacheDemo.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<DenormalizedZipCode> DenormalizedZipCodes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}