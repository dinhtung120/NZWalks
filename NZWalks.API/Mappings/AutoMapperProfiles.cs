using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    /// <summary>
    /// Cấu hình AutoMapper cho việc ánh xạ giữa các đối tượng Domain và DTO
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Cấu hình ánh xạ Region
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>();
            CreateMap<UpdateRegionRequestDto, Region>();

            // Cấu hình ánh xạ Walk
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>();
            CreateMap<UpdateWalkRequestDto, Walk>();

            // Cấu hình ánh xạ Difficulty
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();

            // Cấu hình ánh xạ Image
            CreateMap<Image, ImageUploadRequestDto>().ReverseMap();
        }
    }
}