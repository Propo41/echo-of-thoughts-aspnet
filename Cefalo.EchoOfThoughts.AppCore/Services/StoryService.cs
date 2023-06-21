using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
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

        public async Task<Story> FindById(int id) {
            var story = await _storyRepository.FindById(id);
            if (story == null) {
                throw new NotFoundException("No story is associated with the given id");
            }
            return story;
        }

        public Task<IEnumerable<Story>> GetAll() {
            return _storyRepository.FindAllAsync();
        }

        public async Task<Story> Update(int id, Story story) {
            if (id != story.Id) {
                throw new BadRequestException("Id mismatch");
            }

            var existingStory = await _storyRepository.FindById(id);
            if (existingStory == null || existingStory.Id != id) {
                throw new NotFoundException("No story is associated with the given id");
            }
            return await _storyRepository.Update(story);
        }

    }
}
