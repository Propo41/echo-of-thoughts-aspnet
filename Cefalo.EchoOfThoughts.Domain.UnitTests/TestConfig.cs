using Cefalo.EchoOfThoughts.Domain.Entities;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Cefalo.EchoOfThoughts.Domain.UnitTests {
    public class TestConfig {
        public DbContextOptions<ApplicationDbContext>? DbContextOptions;
        public IConfiguration Config;

        public TestConfig() {
            Config = A.Fake<IConfiguration>();
        }

        public async Task<ApplicationDbContext> CreateDbContextAsync() {
            var context = new ApplicationDbContext(DbContextOptions, Config);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        public void InitializeDbContext(string dbName) {
            DbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        }


        public User CreateDummyUser(int id, string username, int hasStories = 0) {
            var user = new User {
                Id = id,
                Email = "ali@gmail.com",
                PasswordHash = "asd",
                UserName = username,
                CreatedAt = DateTime.Today,
                PasswordUpdatedAt = DateTime.Today,
                FullName = "asd asd",
                IsDisabled = false,
                ProfilePicture = "fake juckerburg image",
                Role = 1,
            };
            if (hasStories <= 0)
                return user;

            var stories = new List<Story>();
            for (var i = 0; i < hasStories; i++) {
                stories.Add(CreateDummyStory(i + 1, i + 1, $"ali{i}"));
            }
            user.Stories = stories;
            return user;
        }

        public Story CreateDummyStory(int id, int authorId = 1, string authorUsername = "ali", bool includeAuthor = false) {
            var story = new Story {
                Id = id,
                Title = "title1",
                Body = "body1",
                PublishedDate = DateTime.Today,
                UpdatedAt = DateTime.Today,
                AuthorId = authorId,
            };

            if (includeAuthor) {
                story.Author = CreateDummyUser(story.AuthorId, authorUsername);
            }

            return story;
        }
    }
}
