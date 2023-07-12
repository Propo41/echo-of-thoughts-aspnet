using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces {
    public interface IAuthHelper {
        public string CreateJwt(User user, IConfiguration configuration);
        public TokenValidationParameters GeTokenValidationParameters(IConfiguration configuration);

        public string HashPassword(string password);
        public bool IsPasswordValid(string password, string hash);
    }
}
