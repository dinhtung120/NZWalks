using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

/// <summary>
/// Interface định nghĩa các phương thức CRUD cho Walk
/// </summary>
public interface IWalkRepository
{
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
    Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);

    /// <summary>
    /// Lấy walk theo ID
    /// </summary>
    /// <param name="id">ID của walk cần lấy</param>
    /// <returns>Walk tương ứng với ID, null nếu không tìm thấy</returns>
    Task<Walk?> GetByIdAsync(Guid id);

    /// <summary>
    /// Tạo walk mới
    /// </summary>
    /// <param name="walk">Thông tin walk cần tạo</param>
    /// <returns>Walk đã được tạo</returns>
    Task<Walk> CreateAsync(Walk walk);

    /// <summary>
    /// Cập nhật thông tin walk
    /// </summary>
    /// <param name="id">ID của walk cần cập nhật</param>
    /// <param name="walk">Thông tin mới của walk</param>
    /// <returns>Walk đã được cập nhật, null nếu không tìm thấy</returns>
    Task<Walk?> UpdateAsync(Guid id, Walk walk);

    /// <summary>
    /// Xóa walk theo ID
    /// </summary>
    /// <param name="id">ID của walk cần xóa</param>
    /// <returns>Walk đã bị xóa, null nếu không tìm thấy</returns>
    Task<Walk?> DeleteAsync(Guid id);
}