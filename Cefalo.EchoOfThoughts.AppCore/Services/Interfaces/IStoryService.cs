using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IStoryService {
        Task<StoryDto> Create(int authorId, StoryDto storyDto);
        Task<IEnumerable<StoryDto>> GetAll(int pageNumber, int pageSize);
        Task<StoryDto> FindById(int id);
        Task<StoryDto> Update(int userId, int blogId, StoryUpdateDto updateDto);
        Task<Payload> DeleteById(int id, int userId);
    }
}