using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IUserService {
        Task<IEnumerable<UserDto>> GetAllAsync(string username);
        Task<UserDto> FindAsync(int id);
        Task<UserDto> FindAsync(string username);
        Task<UserUpdateDto> UpdateAsync(int id, UserUpdateDto updateDto);
        Task<Payload> DeleteByIdAsync(int id);
    }
}
