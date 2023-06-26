using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Cefalo.EchoOfThoughts.WebApi {
    public static class AuthExtensions {
        public static IServiceCollection RegisterAuthServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.TokenValidationParameters = Auth.GeTokenValidationParameters(configuration);
            });

            return services;
        }
    }
}
