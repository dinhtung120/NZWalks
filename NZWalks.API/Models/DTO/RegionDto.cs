using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO;

public class RegionDto
{
    [Required]
    [StringLength(maximumLength:5, MinimumLength = 3, ErrorMessage="Code must be between 3 and 5 characters")]
    public string Code { get; set; }
    [Required]
    [StringLength(maximumLength:20, ErrorMessage="Name has to be a maximum of 20 characters")]
    public string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}