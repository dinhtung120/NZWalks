
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories;

public class SQLRegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SQLRegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Region>> GetAllAsync()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Regions.FindAsync(id);
    }

    public async Task<Region> CreateAsync(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();
        return region;
    }

    public async Task<Region?> UpdateAsync(Guid id, Region region)
    {
        var regionDomain = await _dbContext.Regions.FindAsync(id);
        if (regionDomain == null)
        {
            return null ;
        }
        regionDomain.Name = region.Name;
        regionDomain.Code = region.Code;
        regionDomain.RegionImageUrl = region.RegionImageUrl;
        await _dbContext.SaveChangesAsync();
        return regionDomain;
    }

    public async Task<Region?> DeleteAsync(Guid id)
    {
        var regionDomain = await _dbContext.Regions.FindAsync(id);
        if (regionDomain == null)
        {
            return null;
        }
        _dbContext.Regions.Remove(regionDomain);
        await _dbContext.SaveChangesAsync();
        return regionDomain;
    }
}