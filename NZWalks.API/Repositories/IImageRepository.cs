using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

/// <summary>
/// Interface định nghĩa các phương thức xử lý upload và quản lý hình ảnh
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// Upload hình ảnh lên hệ thống
    /// </summary>
    /// <param name="image">Đối tượng Image chứa thông tin hình ảnh cần upload</param>
    /// <returns>Task chứa đối tượng Image sau khi upload thành công</returns>
    Task<Image> Upload(Image image);
}