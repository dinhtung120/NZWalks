using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    /// <summary>
    /// Controller xử lý các request liên quan đến Walk
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="walkRepository">Repository xử lý Walk</param>
        /// <param name="mapper">AutoMapper để map giữa các đối tượng</param>
        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách tất cả các walk với các tùy chọn lọc, sắp xếp và phân trang
        /// </summary>
        /// <param name="filterOn">Tên trường để lọc</param>
        /// <param name="filterQuery">Giá trị cần lọc</param>
        /// <param name="sortBy">Tên trường để sắp xếp</param>
        /// <param name="isAscending">Sắp xếp tăng dần hay giảm dần</param>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="pageSize">Số phần tử trên mỗi trang</param>
        /// <returns>Danh sách các walk thỏa mãn điều kiện</returns>
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            // Lấy danh sách walk từ repository
            var walksDomain = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            // Map domain model sang DTO
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);

            return Ok(walksDto);
        }

        /// <summary>
        /// Lấy walk theo ID
        /// </summary>
        /// <param name="id">ID của walk cần lấy</param>
        /// <returns>Thông tin walk tương ứng với ID</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Lấy walk từ repository
            var walkDomain = await _walkRepository.GetByIdAsync(id);

            if (walkDomain == null)
            {
                return NotFound();
            }

            // Map domain model sang DTO
            var walkDto = _mapper.Map<WalkDto>(walkDomain);

            return Ok(walkDto);
        }

        /// <summary>
        /// Tạo walk mới
        /// </summary>
        /// <param name="addWalkRequestDto">Thông tin walk cần tạo</param>
        /// <returns>Thông tin walk đã được tạo</returns>
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO sang domain model
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

            // Tạo walk mới
            walkDomainModel = await _walkRepository.CreateAsync(walkDomainModel);

            // Map domain model sang DTO để trả về
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = walkDto.Id }, walkDto);
        }

        /// <summary>
        /// Cập nhật thông tin walk
        /// </summary>
        /// <param name="id">ID của walk cần cập nhật</param>
        /// <param name="updateWalkRequestDto">Thông tin mới của walk</param>
        /// <returns>Thông tin walk đã được cập nhật</returns>
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            // Map DTO sang domain model
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);

            // Cập nhật walk
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map domain model sang DTO để trả về
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }

        /// <summary>
        /// Xóa walk theo ID
        /// </summary>
        /// <param name="id">ID của walk cần xóa</param>
        /// <returns>Thông tin walk đã bị xóa</returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Xóa walk
            var walkDomainModel = await _walkRepository.DeleteAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // Map domain model sang DTO để trả về
            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }
    }
}
