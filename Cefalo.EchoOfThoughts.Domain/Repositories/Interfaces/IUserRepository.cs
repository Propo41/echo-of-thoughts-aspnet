using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces {
    public interface IUserRepository {
        Task<User> CreateAsync(User user);
        Task<IEnumerable<User>> FindAllAsync(string username);
        Task<User> Find(int id);
        Task<User> Find(string username);
        Task<User> FindByEmail(string email);
        Task<User> Update(User user);
        Task<int> DeleteAsync(User user);
    }
}
