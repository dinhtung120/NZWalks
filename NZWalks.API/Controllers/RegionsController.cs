using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomains = await _dbContext.Regions.ToListAsync();
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomains)
            {
                regionsDto.Add(new RegionDto()
                {
                    Code=regionDomain.Code,
                    Name=regionDomain.Name,
                    RegionImageUrl=regionDomain.RegionImageUrl,
                });
            }
            
            return Ok(regionsDto);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await _dbContext.Regions.FindAsync(id);
            //var region = _dbContext.Regions.FirstOrDefault(x =>x.Id == id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = new RegionDto
            {
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
           
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegionDto regionDto)
        {
            var regionDomain = new Region
            {
                Code = regionDto.Code,
                Name = regionDto.Name,
                RegionImageUrl = regionDto.RegionImageUrl,
            };
            await _dbContext.AddAsync(regionDomain);
            await _dbContext.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] RegionDto regionDto)
        {
            var regionDomain = await _dbContext.Regions.FindAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            regionDomain.Code = regionDto.Code;
            regionDomain.Name = regionDto.Name;
            regionDomain.RegionImageUrl = regionDto.RegionImageUrl;
            await _dbContext.SaveChangesAsync();
            
            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomain = await _dbContext.Regions.FindAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            _dbContext.Remove(regionDomain);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
