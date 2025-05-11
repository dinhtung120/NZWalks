using System.ComponentModel.DataAnnotations;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO;

public class WalkDto
{
    [Required]
    [StringLength(maximumLength:100,ErrorMessage = "Name is too long")]
    public string Name { get; set; }
    
    [Required]
    [StringLength(maximumLength:1000,ErrorMessage = "Description is too long")]
    public string Description { get; set; }
    
    [Required]
    public string LengthInKm { get; set; }
    
    public string? WalkImageUrl { get; set; }
    
    [Required]
    public RegionDto Region { get; set; }
    
    [Required]
    public Difficulty Difficulty { get; set; }
    
}