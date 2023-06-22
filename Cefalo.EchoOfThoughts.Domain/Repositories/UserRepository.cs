using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.EchoOfThoughts.Domain.Repositories {
    public class UserRepository : IUserRepository {
        protected readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<User> CreateAsync(User user) {
            var newUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return newUser.Entity;
        }

        public Task<IEnumerable<User>> FindAllAsync() {
            throw new NotImplementedException();
        }

        public async Task<User> FindById(int id) {
            return await _context.Users
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);
        }

        public async Task<User> Update(User user) {
            var updatedUser = _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return updatedUser.Entity;
        }

        public Task<int> DeleteAsync(User user) {
            throw new NotImplementedException();
        }

        public Task<int> UpdateRoles(int userId, int role) {
            throw new NotImplementedException();
        }
    }
}
