using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public static class Auth {
        public static string CreateJwt(User user, IConfiguration configuration) {
            var key = Encoding.ASCII.GetBytes
                (configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.UserName),
                    new Claim("FullName", user.FullName),
                    new Claim("ProfilePicture", user.ProfilePicture),
                    new Claim("Email", user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static TokenValidationParameters GeTokenValidationParameters(IConfiguration configuration) {
            return new TokenValidationParameters {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }

        public static string HashPassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool IsPasswordValid(string password, string hash) {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
