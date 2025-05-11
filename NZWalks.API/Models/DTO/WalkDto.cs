using NZWalks.API.Models.Domain;

namespace NZWalks.API.Models.DTO;

public class WalkDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string LengthInKm { get; set; }
    public string? WalkImageUrl { get; set; }
    
    public RegionDto Region { get; set; }
    public Difficulty Difficulty { get; set; }
    
}