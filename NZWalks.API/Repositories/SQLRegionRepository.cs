using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories;

/// <summary>
/// Implementation của IRegionRepository để thao tác với Region trong SQL Server
/// </summary>
public class SQLRegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    /// <summary>
    /// Constructor khởi tạo repository với database context
    /// </summary>
    /// <param name="dbContext">Context của database</param>
    public SQLRegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Lấy danh sách tất cả các region
    /// </summary>
    /// <returns>Danh sách các region</returns>
    public async Task<List<Region>> GetAllAsync()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    /// <summary>
    /// Lấy region theo ID
    /// </summary>
    /// <param name="id">ID của region cần lấy</param>
    /// <returns>Region tương ứng với ID, null nếu không tìm thấy</returns>
    public async Task<Region?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Tạo region mới
    /// </summary>
    /// <param name="region">Thông tin region cần tạo</param>
    /// <returns>Region đã được tạo</returns>
    public async Task<Region> CreateAsync(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();
        return region;
    }

    /// <summary>
    /// Cập nhật thông tin region
    /// </summary>
    /// <param name="id">ID của region cần cập nhật</param>
    /// <param name="region">Thông tin mới của region</param>
    /// <returns>Region đã được cập nhật, null nếu không tìm thấy</returns>
    public async Task<Region?> UpdateAsync(Guid id, Region region)
    {
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRegion == null)
        {
            return null;
        }

        existingRegion.Code = region.Code;
        existingRegion.Name = region.Name;
        existingRegion.RegionImageUrl = region.RegionImageUrl;

        await _dbContext.SaveChangesAsync();
        return existingRegion;
    }

    /// <summary>
    /// Xóa region theo ID
    /// </summary>
    /// <param name="id">ID của region cần xóa</param>
    /// <returns>Region đã bị xóa, null nếu không tìm thấy</returns>
    public async Task<Region?> DeleteAsync(Guid id)
    {
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        if (existingRegion == null)
        {
            return null;
        }

        _dbContext.Regions.Remove(existingRegion);
        await _dbContext.SaveChangesAsync();
        return existingRegion;
    }
}