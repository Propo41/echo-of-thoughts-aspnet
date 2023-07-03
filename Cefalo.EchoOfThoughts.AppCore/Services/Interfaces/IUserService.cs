using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IUserService {
        Task<UserDto> Create(UserSignUpDto userDto);
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> FindById(int id);
        Task<UserUpdateDto> Update(int id, UserUpdateDto updateDto);
        Task<Payload> DeleteById(int id);
    }
}
