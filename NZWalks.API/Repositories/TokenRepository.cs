using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace NZWalks.API.Repositories;

/// <summary>
/// Implementation của ITokenRepository để tạo và quản lý JWT token
/// </summary>
public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Constructor khởi tạo repository với configuration
    /// </summary>
    /// <param name="configuration">Configuration chứa các cài đặt JWT</param>
    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Tạo JWT token cho người dùng
    /// </summary>
    /// <param name="user">Thông tin người dùng cần tạo token</param>
    /// <param name="roles">Danh sách role của người dùng</param>
    /// <returns>JWT token dạng string</returns>
    public string CreateJWTToken(IdentityUser user, List<string> roles)
    {
        // Tạo claims cho token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        // Thêm claims cho từng role
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Lấy key và issuer từ configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        // Tạo token với các thông tin đã có
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );

        // Trả về token dạng string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}