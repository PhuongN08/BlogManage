using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogManage.Services.AuthenServices
{
    public static class JwtHelper
    {
        public static ClaimsPrincipal? ValidateToken(string token, IConfiguration config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]!);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                return tokenHandler.ValidateToken(token, parameters, out _);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JWT validation failed: {ex.Message}");
                return null;
            }
        }

        public static int GetProfileId(ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst("ProfileId")?.Value;
            return int.TryParse(idClaim, out var id)
                ? id
                : throw new Exception("❌ Claim 'ProfileId' không hợp lệ hoặc không tồn tại.");
        }
    }

}
