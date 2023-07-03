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

        public async Task<IEnumerable<User>> FindAllAsync() {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<User> FindById(int id) {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> FindByEmail(string email) {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> FindByUsername(string username) {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User> Update(User user) {
            var updatedUser = _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return updatedUser.Entity;
        }

        public async Task<int> DeleteAsync(User user) {
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }
    }
}
