using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;

namespace Cefalo.EchoOfThoughts.WebApi.Extensions {
    public static class AuthExtensions {
        public static IServiceCollection RegisterAuthServices(this IServiceCollection services, IConfiguration configuration, IAuthHelper authHelper) {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.TokenValidationParameters = authHelper.GeTokenValidationParameters(configuration);
            });

            return services;
        }
    }
}
