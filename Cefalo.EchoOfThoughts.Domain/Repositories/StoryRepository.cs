﻿using Cefalo.EchoOfThoughts.Domain.Entities;
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

        public async Task<IEnumerable<Story>> FindAllAsync(int position, int pageSize, bool includeAuthor) {
            var storyQuery = _context.Stories.AsQueryable();
            storyQuery = storyQuery
                .OrderBy(d => d.PublishedDate)
                .Skip(position)
                .Take(pageSize);

            if (includeAuthor) {
                storyQuery = storyQuery.Include(s => s.Author);
            }

            return await storyQuery.ToListAsync();
        }

        public async Task<Story> FindById(int id, bool includeAuthor = false) {
            var storyQuery = _context.Stories.AsQueryable();
            if (includeAuthor) {
                storyQuery = storyQuery.Include(s => s.Author);
            }

            return await storyQuery.FirstOrDefaultAsync(x => x.Id == id);
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
