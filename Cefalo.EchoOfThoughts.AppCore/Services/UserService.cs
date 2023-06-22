using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class UserService : IUserService {
        public Task<UserDto> Create(UserSignUpDto signUpDto) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetAll() {
            throw new NotImplementedException();
        }

        public Task<UserDto> FindById(int id) {
            throw new NotImplementedException();
        }

        public Task<UserDto> Update(int id, UserUpdateDto updateDto) {
            throw new NotImplementedException();
        }

        public Task<Payload> DeleteById(int id) {
            throw new NotImplementedException();
        }

        public Task<Payload> UpdateRole(int id, string[] roles) {
            throw new NotImplementedException();
        }
    }
}
