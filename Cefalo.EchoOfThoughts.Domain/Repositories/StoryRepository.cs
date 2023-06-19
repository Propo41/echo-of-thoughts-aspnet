using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.EchoOfThoughts.Domain.Repositories {
    public class StoryRepository : IStoryRepository {

        protected readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Story> AddAsync(Story story) {
            try {
                var res = await _context.Stories.AddAsync(story);
                var count = await _context.SaveChangesAsync();
                Console.WriteLine(count);
                return res.Entity;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return null;
            }

        }

        public async Task<IEnumerable<Story>> FindAllAsync() {
            return await _context.Stories.ToListAsync();
        }

    }
}
