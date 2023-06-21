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

        public async Task<StoryDto> Create(StoryDto storyDto) {
            var storyEntity = _mapper.Map<Story>(storyDto);
            var createdStory = await _storyRepository.AddAsync(storyEntity);
            return _mapper.Map<StoryDto>(createdStory);
        }

        public async Task<StoryDto> FindById(int id) {
            var story = await _storyRepository.FindById(id);
            return story == null
                ? throw new NotFoundException("No story is associated with the given id")
                : _mapper.Map<StoryDto>(story);
        }

        public async Task<IEnumerable<StoryDto>> GetAll() {
            var stories = await _storyRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<StoryDto>>(stories);
        }

        public async Task<StoryDto> Update(int id, StoryUpdateDto storyDto) {
            var existingStory = await _storyRepository.FindById(id);
            if (existingStory == null) {
                throw new NotFoundException("No story is associated with the given id");
            }
            var storyEntity = _mapper.Map<Story>(storyDto);
            storyEntity.Id = id;
            storyEntity.PublishedDate = existingStory.PublishedDate;
            var story = await _storyRepository.Update(storyEntity);
            return _mapper.Map<StoryDto>(story);
        }

        public async Task<Payload> DeleteById(int id) {
            var existingStory = await _storyRepository.FindById(id);
            if (existingStory == null) {
                throw new NotFoundException("No story is associated with the given id");
            }

            await _storyRepository.DeleteAsync(existingStory);
            return new Payload("Deleted successfully");
        }
    }
}
