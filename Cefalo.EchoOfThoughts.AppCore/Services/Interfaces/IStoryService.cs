using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IStoryService {

        Task<Story> Create(Story story);
        Task<Story> Update(Story story);
        Task<Story> Delete(Story story);
        Task<Story> Get(Story story);

    }
}
