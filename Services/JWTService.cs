using Microsoft.IdentityModel.Tokens;
using SmartAgroAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartAgroAPI.Services
{
    public static class JWTService
    {

        public static string GenerateToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = "SmartAgroJwtTokenSecurityKeyGrupo6"u8.ToArray();

            var claims = new List<Claim>()
            {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Name, user.Nome),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new ("isAdmin", user.IsAdmin.ToString().ToLower())
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
