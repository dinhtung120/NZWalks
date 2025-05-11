using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await _walkRepository.GetAllAsync(filterOn, filterQuery,
                sortBy, isAscending ?? true, pageNumber, pageSize);
            var walksDto = _mapper.Map<List<WalkDto>>(walksDomain);
            return Ok(walksDto);
        }


        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var walkDomain = await _walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkDto = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkDto);
        }
        
        
        
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]WalkDto walkDto)
        {
        
                var walk = _mapper.Map<Walk>(walkDto);
                await _walkRepository.CreateAsync(walk);
                var walkToReturn = _mapper.Map<WalkDto>(walk);
                return Ok(walkToReturn);
        
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] WalkDto walkDto)
        {
        
                var walk = _mapper.Map<Walk>(walkDto);
                var walkDomain = await _walkRepository.UpdateAsync(id, walk);
                if (walkDomain == null)
                {
                    return NotFound();
                }
                var walkToReturn = _mapper.Map<WalkDto>(walkDomain);
                return Ok(walkToReturn);
                
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walkDomain = await _walkRepository.DeleteAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            var walkToReturn = _mapper.Map<WalkDto>(walkDomain);
            return Ok(walkToReturn);
        }
    }
}
