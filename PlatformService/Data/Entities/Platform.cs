using System.ComponentModel.DataAnnotations;

namespace PlatformService.Data.Entities;

public class Platform
{
    [Key] public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Publisher { get; set; }
    public required string Cost { get; set; }
}