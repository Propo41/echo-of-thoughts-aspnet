using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class StoryService : IStoryService {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        public StoryService(IStoryRepository storyRepository, IMapper mapper) {
            _mapper = mapper;
            _storyRepository = storyRepository;
        }

        public async Task<StoryDto> Create(int authorId, StoryDto storyDto) {
            var storyEntity = _mapper.Map<Story>(storyDto);
            storyEntity.AuthorId = authorId;
            var createdStory = await _storyRepository.AddAsync(storyEntity);
            return _mapper.Map<StoryDto>(createdStory);
        }

        public async Task<StoryDto> FindById(int id) {
            var story = await _storyRepository.FindById(id, true);
            return story == null
                ? throw new NotFoundException("No story is associated with the given id")
                : _mapper.Map<StoryDto>(story);
        }

        public async Task<IEnumerable<StoryDto>> GetAll() {
            var stories = await _storyRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<StoryDto>>(stories);
        }

        public async Task<StoryDto> Update(int userId, int blogId, StoryUpdateDto storyDto) {
            var existingStory = await _storyRepository.FindById(blogId);
            if (existingStory == null) {
                throw new NotFoundException("No story is associated with the given id");
            }

            if (!existingStory.AuthorId.Equals(userId)) {
                throw new UnauthorizedException("You do not have the privilege to update this story");
            }

            existingStory.Title = storyDto.Title;
            existingStory.Body = storyDto.Body;

            var story = await _storyRepository.Update(existingStory);

            return _mapper.Map<StoryDto>(story);
        }

        public async Task<Payload> DeleteById(int id, int userId) {
            var existingStory = await _storyRepository.FindById(id);
            if (existingStory == null) {
                throw new NotFoundException("No story is associated with the given id");
            }

            if (!existingStory.AuthorId.Equals(userId)) {
                throw new UnauthorizedException("You do not have the privilege to delete this story");
            }

            await _storyRepository.DeleteAsync(existingStory);
            return new Payload("Deleted successfully");
        }
    }
}
