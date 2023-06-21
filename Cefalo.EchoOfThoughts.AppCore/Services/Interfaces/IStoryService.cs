using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Helpers;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IStoryService {
        Task<StoryDto> Create(StoryDto storyDto);
        Task<IEnumerable<StoryDto>> GetAll();
        Task<StoryDto> FindById(int id);
        Task<StoryDto> Update(int id, StoryUpdateDto updateDto);
        Task<Payload> DeleteById(int id);
    }
}