using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories;

/// <summary>
/// Interface định nghĩa các phương thức CRUD cho Region
/// </summary>
public interface IRegionRepository
{
    /// <summary>
    /// Lấy danh sách tất cả các region
    /// </summary>
    /// <returns>Danh sách các region</returns>
    Task<List<Region>> GetAllAsync();

    /// <summary>
    /// Lấy region theo ID
    /// </summary>
    /// <param name="id">ID của region cần lấy</param>
    /// <returns>Region tương ứng với ID, null nếu không tìm thấy</returns>
    Task<Region?> GetByIdAsync(Guid id);

    /// <summary>
    /// Tạo region mới
    /// </summary>
    /// <param name="region">Thông tin region cần tạo</param>
    /// <returns>Region đã được tạo</returns>
    Task<Region> CreateAsync(Region region);

    /// <summary>
    /// Cập nhật thông tin region
    /// </summary>
    /// <param name="id">ID của region cần cập nhật</param>
    /// <param name="region">Thông tin mới của region</param>
    /// <returns>Region đã được cập nhật, null nếu không tìm thấy</returns>
    Task<Region?> UpdateAsync(Guid id, Region region);

    /// <summary>
    /// Xóa region theo ID
    /// </summary>
    /// <param name="id">ID của region cần xóa</param>
    /// <returns>Region đã bị xóa, null nếu không tìm thấy</returns>
    Task<Region?> DeleteAsync(Guid id);
}