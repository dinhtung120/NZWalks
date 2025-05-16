using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    /// <summary>
    /// Controller xử lý các request liên quan đến Region
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        /// <summary>
        /// Constructor khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="regionRepository">Repository xử lý Region</param>
        /// <param name="mapper">AutoMapper để map giữa các đối tượng</param>
        /// <param name="logger">Logger để ghi log</param>
        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách tất cả các region
        /// </summary>
        /// <returns>Danh sách các region</returns>
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            // Lấy danh sách region từ repository
            var regionsDomain = await _regionRepository.GetAllAsync();
            
            // Map domain model sang DTO
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            
            return Ok(regionsDto);
        }
        
        /// <summary>
        /// Lấy region theo ID
        /// </summary>
        /// <param name="id">ID của region cần lấy</param>
        /// <returns>Thông tin region tương ứng với ID</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Lấy region từ repository
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map domain model sang DTO
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
           
            return Ok(regionDto);
        }

        /// <summary>
        /// Tạo region mới
        /// </summary>
        /// <param name="addRegionRequestDto">Thông tin region cần tạo</param>
        /// <returns>Thông tin region đã được tạo</returns>
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map DTO sang domain model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            // Tạo region mới
            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);
            // Map domain model sang DTO để trả về
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            
            // Trả về kết quả với status 201 Created
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        /// <summary>
        /// Cập nhật thông tin region
        /// </summary>
        /// <param name="id">ID của region cần cập nhật</param>
        /// <param name="updateRegionRequestDto">Thông tin mới của region</param>
        /// <returns>Thông tin region đã được cập nhật</returns>
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO sang domain model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);
            // Cập nhật region
            var regionDomain = await _regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map domain model sang DTO để trả về
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            
            return Ok(regionDto);
        }

        /// <summary>
        /// Xóa region theo ID
        /// </summary>
        /// <param name="id">ID của region cần xóa</param>
        /// <returns>Thông tin region đã bị xóa</returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Xóa region
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // Map domain model sang DTO để trả về
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
