using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces {
    public interface IStoryRepository {
        Task<Story> AddAsync(Story story);
        Task<(int, IEnumerable<Story>)> FindAllAsync(int position, int pageSize, bool includeAuthor = false);
        Task<Story> FindByIdAsync(int id, bool includeAuthor = false);
        Task<Story> UpdateAsync(Story story);
        Task<int> DeleteAsync(Story story);
    }
}
