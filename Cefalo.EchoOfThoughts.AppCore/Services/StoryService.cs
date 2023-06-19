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
            var res = await _storyRepository.AddAsync(story);
            return res;
        }

        public Task<Story> Delete(Story story) {
            throw new NotImplementedException();
        }

        public Task<Story> Get(Story story) {
            throw new NotImplementedException();
        }

        public Task<Story> Update(Story story) {
            throw new NotImplementedException();
        }
    }
}
