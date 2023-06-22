﻿using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces {
    public interface IUserRepository {
        Task<User> CreateAsync(User user);
        Task<IEnumerable<User>> FindAllAsync();
        Task<User> FindById(int id);
        Task<User> Update(User user);
        Task<int> DeleteAsync(User user);
    }
}