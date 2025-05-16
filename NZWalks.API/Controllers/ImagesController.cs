using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    /// <summary>
    /// Controller xử lý các request liên quan đến hình ảnh
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        /// <summary>
        /// Constructor khởi tạo controller với image repository
        /// </summary>
        /// <param name="imageRepository">Repository xử lý hình ảnh</param>
        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        /// <summary>
        /// Upload hình ảnh lên hệ thống
        /// </summary>
        /// <param name="request">Thông tin hình ảnh cần upload</param>
        /// <returns>Thông tin hình ảnh sau khi upload thành công</returns>
        [HttpPost]
        [Route("Upload")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            // Validate file
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                // Chuyển đổi request thành domain model
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                };

                // Upload hình ảnh
                var image = await _imageRepository.Upload(imageDomainModel);

                // Chuyển đổi domain model thành DTO để trả về
                var response = new ImageDto
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    FileDescription = image.FileDescription,
                    FileExtension = image.FileExtension,
                    FilePath = image.FilePath,
                };

                return Ok(response);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Validate thông tin file upload
        /// </summary>
        /// <param name="request">Thông tin file cần validate</param>
        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            // Danh sách các định dạng file được phép
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            // Kiểm tra định dạng file
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            // Kiểm tra kích thước file (10MB)
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload a smaller size file.");
                ModelState.AddModelError("File", "File size more than 10MB, Please upload a smaller size file");
            }
        }
    }
}
