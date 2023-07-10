using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces {
    public interface IUserRepository {
        Task<User> CreateAsync(User user);
        Task<IEnumerable<User>> FindAllAsync(string username);
        Task<User> FindAsync(int id);
        Task<User> FindAsync(string username);
        Task<User> FindByEmailAsync(string email);
        Task<User> UpdateAsync(User user);
        Task<int> DeleteAsync(User user);
    }
}
