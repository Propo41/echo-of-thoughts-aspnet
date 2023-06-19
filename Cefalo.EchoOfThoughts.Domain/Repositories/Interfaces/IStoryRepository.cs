using Cefalo.EchoOfThoughts.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces {
    public interface IStoryRepository {

        Task<Story> AddAsync(Story story);

        Task<Story> UpdateAsync(Story story);

        Task<Story> DeleteAsync(int id);

        Task<Story> FindByIdAsync(int id);

        Task<IEnumerable<Story>> FindAllAsync();

    }
}
