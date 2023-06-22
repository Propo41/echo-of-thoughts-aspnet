using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.Domain.Repositories {
    public class UserRepository : IUserRepository {
        public Task<User> CreateAsync(User user) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> FindAllAsync() {
            throw new NotImplementedException();
        }

        public Task<User> FindById(int id) {
            throw new NotImplementedException();
        }

        public Task<User> Update(User user) {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(User user) {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRoles(int userId, int role) {
            throw new NotImplementedException();
        }
    }
}
