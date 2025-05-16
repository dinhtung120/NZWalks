using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    /// <summary>
    /// Controller xử lý các request liên quan đến xác thực và phân quyền
    /// Cung cấp các API đăng ký và đăng nhập
    /// Sử dụng ASP.NET Core Identity và JWT token
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// Constructor khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="userManager">Quản lý người dùng</param>
        /// <param name="tokenRepository">Repository xử lý token</param>
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        
        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        /// <param name="request">Thông tin đăng ký</param>
        /// <returns>Thông tin tài khoản đã đăng ký</returns>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Tạo đối tượng IdentityUser từ request
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username
            };

            // Tạo tài khoản mới
            var identityResult = await _userManager.CreateAsync(identityUser, request.Password);

            if (identityResult.Succeeded)
            {
                // Thêm role cho người dùng
                if (request.Roles != null && request.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, request.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }

            return BadRequest("Something went wrong");
        }

        /// <summary>
        /// Đăng nhập vào hệ thống
        /// </summary>
        /// <param name="request">Thông tin đăng nhập</param>
        /// <returns>JWT token nếu đăng nhập thành công</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // Tìm người dùng theo username
            var user = await _userManager.FindByEmailAsync(request.Username);

            if (user != null)
            {
                // Kiểm tra mật khẩu
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, request.Password);

                if (checkPasswordResult)
                {
                    // Lấy danh sách role của người dùng
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Tạo JWT token
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        // Tạo response
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect");
        }
    }
}
