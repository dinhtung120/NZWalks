using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

/// <summary>
/// Implementation của IWalkRepository để thao tác với Walk trong SQL Server
/// </summary>
public class SQLWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    /// <summary>
    /// Constructor khởi tạo repository với database context
    /// </summary>
    /// <param name="dbContext">Context của database</param>
    public SQLWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
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
    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
    {
        // Lấy tất cả walk và include các navigation property
        var walks = _dbContext.Walks
            .Include("Difficulty")
            .Include("Region")
            .AsQueryable();

        // Áp dụng filter nếu có
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(x => x.Name.Contains(filterQuery));
            }
        }

        // Áp dụng sorting nếu có
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            }
            else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
            }
        }

        // Áp dụng phân trang
        var skipResults = (pageNumber - 1) * pageSize;
        return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
    }

    /// <summary>
    /// Lấy walk theo ID
    /// </summary>
    /// <param name="id">ID của walk cần lấy</param>
    /// <returns>Walk tương ứng với ID, null nếu không tìm thấy</returns>
    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Walks
            .Include("Difficulty")
            .Include("Region")
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Tạo walk mới
    /// </summary>
    /// <param name="walk">Thông tin walk cần tạo</param>
    /// <returns>Walk đã được tạo</returns>
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();
        return walk;
    }

    /// <summary>
    /// Cập nhật thông tin walk
    /// </summary>
    /// <param name="id">ID của walk cần cập nhật</param>
    /// <param name="walk">Thông tin mới của walk</param>
    /// <returns>Walk đã được cập nhật, null nếu không tìm thấy</returns>
    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null)
        {
            return null;
        }

        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.LengthInKm = walk.LengthInKm;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;

        await _dbContext.SaveChangesAsync();
        return existingWalk;
    }

    /// <summary>
    /// Xóa walk theo ID
    /// </summary>
    /// <param name="id">ID của walk cần xóa</param>
    /// <returns>Walk đã bị xóa, null nếu không tìm thấy</returns>
    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingWalk == null)
        {
            return null;
        }

        _dbContext.Walks.Remove(existingWalk);
        await _dbContext.SaveChangesAsync();
        return existingWalk;
    }
}