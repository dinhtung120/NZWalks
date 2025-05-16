using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

/// <summary>
/// Implementation của IImageRepository để lưu trữ hình ảnh trên local filesystem
/// </summary>
public class LocalImageRepository : IImageRepository
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly NZWalksDbContext _dbContext;

    /// <summary>
    /// Constructor khởi tạo repository với các dependency cần thiết
    /// </summary>
    /// <param name="webHostEnvironment">Môi trường hosting của ứng dụng</param>
    /// <param name="httpContextAccessor">Truy cập HttpContext</param>
    /// <param name="dbContext">Context của database</param>
    public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Upload hình ảnh lên local filesystem
    /// </summary>
    /// <param name="image">Đối tượng Image chứa thông tin hình ảnh cần upload</param>
    /// <returns>Task chứa đối tượng Image sau khi upload thành công</returns>
    public async Task<Image> Upload(Image image)
    {
        // Tạo đường dẫn thư mục lưu trữ hình ảnh
        var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images",
            $"{image.FileName}{image.FileExtension}");

        // Upload file lên local filesystem
        using var stream = new FileStream(localFilePath, FileMode.Create);
        await image.File.CopyToAsync(stream);

        // Tạo URL để truy cập hình ảnh
        var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
        image.FilePath = urlFilePath;
        
        await _dbContext.Images.AddAsync(image);
        await _dbContext.SaveChangesAsync();
        return image;
    }
}