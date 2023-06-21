using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces {
    public interface IStoryRepository {
        Task<Story> AddAsync(Story story);
        Task<IEnumerable<Story>> FindAllAsync();
    }
}
