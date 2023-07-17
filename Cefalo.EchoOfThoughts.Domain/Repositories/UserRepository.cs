using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Cefalo.EchoOfThoughts.Domain.Repositories {
    public class UserRepository : IUserRepository {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<User> CreateAsync(User user) {
            var newUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return newUser.Entity;
        }

        public async Task<IEnumerable<User>> FindAllAsync(string username) {
            var userQuery = _context.Users.AsQueryable();
            if (!username.IsNullOrEmpty()) {
                userQuery = userQuery.Where(u => u.UserName.Contains(username));
            }

            return await userQuery.ToListAsync();
        }

        public async Task<User> FindAsync(int id) {
            return await _context.Users
                .Include(s => s.Stories)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> FindAsync(string username) {
            return await _context.Users
                .Include(s => s.Stories)
                .FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<User> FindByEmailAsync(string email) {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> UpdateAsync(User user) {
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
