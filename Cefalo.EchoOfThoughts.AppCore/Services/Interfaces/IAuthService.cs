using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IAuthService {
        Task<string> SignIn(UserSignInDto userSignDto);
        Task<Tuple<UserDto, string>> Create(UserSignUpDto userDto);
        Task<UserDto> FindByEmail(string email);
        Task<Payload> Delete(int id);
        Task<Payload> UpdatePassword(int userId, UserPasswordDto passwordDto); // maps string roles to int
        Task<Payload> SignOut();
    }
}