using FusionCacheDemo.Api.Configuration;
using FusionCacheDemo.Core.Interfaces;
using FusionCacheDemo.Infrastructure.Caching;
using FusionCacheDemo.Infrastructure.Data;
using FusionCacheDemo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
    
// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped<IDenormalizedZipCodeRepository, DenormalizedZipCodeRepository>();

// Register cache services
builder.Services.AddScoped<InMemoryCacheService>();
builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddScoped<HybridCacheService>();

// Cache Configuration
builder.Services.AddMemoryCache();
builder.Services.AddCachingServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();