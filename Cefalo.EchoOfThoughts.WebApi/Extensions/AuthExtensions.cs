using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cefalo.EchoOfThoughts.WebApi.Extensions {
    public static class AuthExtensions {
        private static TokenValidationParameters GeTokenValidationParameters(IConfiguration configuration) {
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
        public static IServiceCollection RegisterAuthServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.TokenValidationParameters = GeTokenValidationParameters(configuration);
            });

            return services;
        }
    }
}
