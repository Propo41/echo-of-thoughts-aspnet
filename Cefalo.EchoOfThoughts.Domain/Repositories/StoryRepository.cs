using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cefalo.EchoOfThoughts.Domain.Repositories {
    public class StoryRepository : IStoryRepository {

        protected readonly ApplicationDbContext _context;

        public StoryRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Story> AddAsync(Story story) {
            var res = await _context.Stories.AddAsync(story);
            return res.Entity;
        }

        public Task<Story> DeleteAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Story>> FindAllAsync() {
            throw new NotImplementedException();
        }

        public Task<Story> FindByIdAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<Story> UpdateAsync(Story story) {
            throw new NotImplementedException();
        }
    }
}
