using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IUserService {
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> Find(int id);
        Task<UserDto> Find(string username);
        Task<UserUpdateDto> Update(int id, UserUpdateDto updateDto);
        Task<Payload> DeleteById(int id);
    }
}
