using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartAgroAPI.Services
{
    public static class JWTService
    {

        public static string GenerateToken(Guid userId, string email, bool userAuth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "SmartAgroSecurityKey"u8.ToArray();

            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, userId.ToString()),
                new (JwtRegisteredClaimNames.Email, email),
                new ("isAdmin", userAuth.ToString())
            };

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = "SmartAgro",
                Audience = "SmartAgroAudience",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
