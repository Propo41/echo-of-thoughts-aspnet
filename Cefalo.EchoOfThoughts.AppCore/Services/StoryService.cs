using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class StoryService : IStoryService {

        private readonly IStoryRepository _storyRepository;

        public StoryService(IStoryRepository storyRepository) {
            _storyRepository = storyRepository;
        }

        public async Task<Story> Create(Story story) {
            return await _storyRepository.AddAsync(story);
        }

        public Task<IEnumerable<Story>> GetAll() {
            return _storyRepository.FindAllAsync();
        }
    }
}
