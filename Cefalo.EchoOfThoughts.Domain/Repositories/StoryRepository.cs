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
            var res = await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<IEnumerable<Story>> FindAllAsync() {
            return await _context.Stories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Story> FindById(int id) {
            var story = await _context.Stories
                .AsNoTracking()
                .FirstAsync(x => x.Id == id);
            return story;
        }

        public async Task<Story> Update(Story story) {
            var updatedStory = _context.Stories.Update(story);
            await _context.SaveChangesAsync();
            return updatedStory.Entity;
        }

        public async Task<int> DeleteAsync(Story story) {
            _context.Stories.Remove(story);
            return await _context.SaveChangesAsync();
        }
    }
}
