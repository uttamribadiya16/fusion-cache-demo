using FusionCacheDemo.Core.Entities;

namespace FusionCacheDemo.Infrastructure.Shared;

public class Constants
{
    public static readonly List<DenormalizedZipCode> SampleZipCodes = new()
    {
        new DenormalizedZipCode
        {
            Id = Guid.NewGuid(),
            StateId = 1,
            CountyId = 1,
            StateName = "New York",
            StateCode = "NY",
            CountyName = "New York County",
            CityName = "New York",
            ZipCode = "10001",
            CreatedBy = 1,
            LastModifiedBy = 1,
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        },
        new DenormalizedZipCode
        {
            Id = Guid.NewGuid(),
            StateId = 2,
            CountyId = 2,
            StateName = "California",
            StateCode = "CA",
            CountyName = "Los Angeles County",
            CityName = "Los Angeles",
            ZipCode = "90001",
            CreatedBy = 2,
            LastModifiedBy = 2,
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        }
    };
    
    public static readonly List<DenormalizedZipCode> UpdatedSampleZipCodes = new()
    {
        new DenormalizedZipCode
        {
            Id = Guid.NewGuid(),
            StateId = 3,
            CountyId = 3,
            StateName = "Texas",
            StateCode = "TX",
            CountyName = "Harris County",
            CityName = "Houston",
            ZipCode = "77001",
            CreatedBy = 3,
            LastModifiedBy = 3,
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        },
        new DenormalizedZipCode
        {
            Id = Guid.NewGuid(),
            StateId = 4,
            CountyId = 4,
            StateName = "Florida",
            StateCode = "FL",
            CountyName = "Miami-Dade County",
            CityName = "Miami",
            ZipCode = "33101",
            CreatedBy = 4,
            LastModifiedBy = 4,
            CreatedAt = DateTime.UtcNow,
            LastModifiedAt = DateTime.UtcNow
        }
    };
}