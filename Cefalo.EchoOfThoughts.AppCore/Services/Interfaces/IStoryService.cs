using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IStoryService {
        Task<StoryDto> CreateAsync(int authorId, StoryCreateDto storyDto);
        Task<StoriesDto> GetAllAsync(int pageNumber, int pageSize);
        Task<StoryDto> FindByIdAsync(int id);
        Task<StoryDto> UpdateAsync(int userId, int blogId, StoryUpdateDto updateDto);
        Task<Payload> DeleteByIdAsync(int id, int userId);
    }
}