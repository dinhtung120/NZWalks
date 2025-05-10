using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomains = await _regionRepository.GetAllAsync();

            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomains);
            return Ok(regionsDto);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            //var region = _dbContext.Regions.FirstOrDefault(x =>x.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
           
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegionDto regionDto)
        {
            var regionDomain = _mapper.Map<Region>(regionDto);
            regionDomain = await _regionRepository.CreateAsync(regionDomain);
            var regionToReturn = _mapper.Map<RegionDto>(regionDomain);
            
            return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionToReturn);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RegionDto regionDto)
        {
           var regionDomain = await _regionRepository.UpdateAsync(id, regionDto);
           if (regionDomain == null)
           {
               return NotFound();
           }
           var regionToReturn = _mapper.Map<RegionDto>(regionDomain);
            
           return Ok(regionToReturn);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionToReturn = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionToReturn);
        }
    }
}
