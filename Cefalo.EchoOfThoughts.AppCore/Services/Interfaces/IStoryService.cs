using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.AppCore.Services.Interfaces {
    public interface IStoryService {
        Task<Story> Create(Story story);
        Task<IEnumerable<Story>> GetAll();
    }
}
