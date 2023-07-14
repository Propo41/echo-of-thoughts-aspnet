using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class StoryService : IStoryService {
        private readonly IStoryRepository _storyRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public StoryService(IStoryRepository storyRepository, IMapper mapper, IDateTimeProvider dateTimeProvider) {
            _mapper = mapper;
            _storyRepository = storyRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<StoryDto> CreateAsync(int authorId, StoryCreateDto storyDto) {
            var storyEntity = _mapper.Map<Story>(storyDto);
            storyEntity.AuthorId = authorId;
            var currentTime = _dateTimeProvider.GetCurrentTime();
            storyEntity.PublishedDate = currentTime;
            storyEntity.UpdatedAt = currentTime;

            var createdStory = await _storyRepository.AddAsync(storyEntity);
            return _mapper.Map<StoryDto>(createdStory);
         }

        public async Task<StoryDto> FindByIdAsync(int id) {
            var story = await _storyRepository.FindByIdAsync(id, true);
            return story == null
                ? throw new NotFoundException("No story is associated with the given id")
                : _mapper.Map<StoryDto>(story);
        }

        public async Task<StoriesDto> GetAllAsync(int pageNumber, int pageSize) {
            var currentPosition = (pageNumber - 1) * pageSize;
            var (totalCount, stories) = await _storyRepository.FindAllAsync(currentPosition, pageSize, true);
            var storiesDto = _mapper.Map<IEnumerable<StoryDto>>(stories);
            return new StoriesDto {
                Stories = storiesDto,
                TotalCount = totalCount
            };
        }

        public async Task<StoryDto> UpdateAsync(int userId, int blogId, StoryUpdateDto storyDto) {
            var existingStory = await _storyRepository.FindByIdAsync(blogId);
            if (existingStory == null) {
                throw new NotFoundException("No story is associated with the given id");
            }

            if (!existingStory.AuthorId.Equals(userId)) {
                throw new UnauthorizedException("You do not have the privilege to update this story");
            }

            var currentTime = _dateTimeProvider.GetCurrentTime();

            existingStory.Title = storyDto.Title;
            existingStory.Body = storyDto.Body;
            existingStory.PublishedDate = currentTime;
            existingStory.UpdatedAt = currentTime;

            var story = await _storyRepository.UpdateAsync(existingStory);
            return _mapper.Map<StoryDto>(story);
        }

        public async Task<Payload> DeleteByIdAsync(int id, int userId) {
            var existingStory = await _storyRepository.FindByIdAsync(id);
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
