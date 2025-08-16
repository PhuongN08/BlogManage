using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogManage.Models;

namespace BlogManage.Services.AuthenServices
{
    public class JwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Profile profile)
        {
            string normalizedRole = NormalizeRole(profile.Role?.RoleName ?? "Writer");

            var claims = new[]
            {
        new Claim("ProfileId", profile.Id.ToString()),
        new Claim("AccountId", profile.AccountId.ToString()),
        new Claim(ClaimTypes.Role, normalizedRole),
        new Claim("RoleName", normalizedRole)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return "Writer";

            return role.Trim().ToLower() switch
            {
                "admin" => "Admin",
                "writer" => "Writer",
                "manager" => "Manager",
                _ => "Writer"
            };
        }

    }
}
