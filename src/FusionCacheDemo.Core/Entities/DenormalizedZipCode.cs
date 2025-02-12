using System.ComponentModel.DataAnnotations.Schema;

namespace FusionCacheDemo.Core.Entities;

[Table("DenormalizedZipCode")]

public class DenormalizedZipCode
{
    public Guid Id { get; set; } 
    public int StateId { get; set; } 
    public int CountyId { get; set; } 
    public string StateName { get; set; } = string.Empty; 
    public string StateCode { get; set; } = string.Empty; 
    public string? CountyName { get; set; } 
    public string CityName { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty; 
    public long CreatedBy { get; set; } 
    public long LastModifiedBy { get; set; } 
    public DateTime? CreatedAt { get; set; } 
    public DateTime? LastModifiedAt { get; set; } 
}