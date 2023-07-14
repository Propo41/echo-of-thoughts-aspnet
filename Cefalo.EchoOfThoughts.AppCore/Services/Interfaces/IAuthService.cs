using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IAuthService {
        Task<string> SignInAsync(UserSignInDto userSignInDto);
        Task<UserDto> CreateAsync(UserSignUpDto userDto);
        Task<Payload> UpdatePasswordAsync(int userId, UserPasswordDto passwordDto); // maps string roles to int
    }
}