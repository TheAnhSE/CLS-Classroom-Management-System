using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CLS.BackendAPI.Exceptions;
using CLS.BackendAPI.Models;
using CLS.BackendAPI.Models.DTOs.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CLS.BackendAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ClsDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ClsDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || user.PasswordHash != request.Password) // Simple check for MVP
            {
                throw new ValidationException("Email hoặc mật khẩu không chính xác.");
            }

            if (user.IsActive == false)
            {
                throw new ValidationException("Tài khoản đã bị vô hiệu hóa.");
            }

            var token = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                UserId = user.UserId,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = user.Role.RoleName
            };
        }

        private string GenerateJwtToken(Models.Entities.User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // 2 hours expiration
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
