using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.EchoOfThoughts.Domain.UnitTests {
    public class StoryRepositoryTests : TestConfig {
        public StoryRepositoryTests() {
            var dbName = $"StoriesDb_{DateTime.Now.ToFileTimeUtc()}";
            InitializeDbContext(dbName);
        }

        [Fact]
        public async void AddAsync_WithRequiredFields_ReturnsNewStory() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var newStory = CreateDummyStory(1);

            // Act
            var createdStory = await sut.AddAsync(newStory);

            // Assert
            Assert.Equal(1, context.Stories.Count());
            Assert.Equal(createdStory, newStory);
        }

        [Fact]
        public async void AddAsync_WithMissingFields_ThrowsDbException() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var newStory = new Story {
                Id = 1
            };

            // Act
            var exception = await Record.ExceptionAsync(() => sut.AddAsync(newStory));

            // Assert
            Assert.IsType<DbUpdateException>(exception);
        }


        [Fact]
        public async void FindAllAsync_WhereNoStoriesExists_ReturnsEmptyList() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            const int position = 1;
            const int pageSize = 10;
            const bool includeAuthor = false;
            // Act
            var result = await sut.FindAllAsync(position, pageSize, includeAuthor);

            // Assert
            Assert.Equal(0, result.Item1);
            Assert.Empty(result.Item2);
        }

        [Theory]
        [InlineData(1, 1, 2, true)]
        [InlineData(2, 2, 3, false)]
        [InlineData(10, 3, 3, true)]
        public async void FindAllAsync_WhereStoriesExists_ReturnsStoriesWithPagination(int pageSize, int expectedItemsInPage, int totalCount, bool includeAuthor) {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            const int position = 0;
            for (var i = 0; i < totalCount; i++) {
                context.Stories.Add(CreateDummyStory(i + 1, i + 2, $"ali{i}", includeAuthor));

            }
            await context.SaveChangesAsync();
            // Act
            var result = await sut.FindAllAsync(position, pageSize, includeAuthor);

            // Assert
            Assert.Equal(totalCount, result.Item1);
            Assert.NotEmpty(result.Item2);
            Assert.Equal(expectedItemsInPage, result.Item2.Count());
            if (!includeAuthor)
                return;
            foreach (var story in context.Stories) {
                Assert.NotNull(story.Author);
                Assert.IsType<User>(story.Author);
                Assert.Equal(story.AuthorId, story.Author.Id);
            }
        }

        [Fact]
        public async void FindByIdAsync_WhereIdIsNonExistent_ReturnsNull() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            const int id = 100;
            // Act
            var result = await sut.FindByIdAsync(id);
            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(3, true)]
        public async void FindByIdAsync_WhereIdExists_ReturnsStory(int storyId, bool includeAuthor) {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var story = CreateDummyStory(storyId, includeAuthor: includeAuthor);
            context.Stories.Add(story);
            await context.SaveChangesAsync();
            // Act
            var result = await sut.FindByIdAsync(storyId, includeAuthor);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(story, result);
            if (!includeAuthor)
                return;
            Assert.NotNull(result.Author);
            Assert.IsType<User>(result.Author);
        }

        [Fact]
        public async void UpdateAsync_WithRequiredFields_ReturnsUpdatedStory() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var dummyStory = CreateDummyStory(1);
            context.Stories.Add(dummyStory);
            await context.SaveChangesAsync();

            dummyStory.Title = "updated title";
            dummyStory.Body = "updated body";

            // Act
            var result = await sut.UpdateAsync(dummyStory);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(dummyStory.Title, result.Title);
            Assert.Equal(dummyStory.Body, result.Body);
        }

        [Fact]
        public async void UpdateAsync_WithNonExistingStory_ThrowsDbUpdateException() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var dummyStory = CreateDummyStory(1);
            // Act
            var exception = await Record.ExceptionAsync(() => sut.UpdateAsync(dummyStory));
            // Assert
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }


        [Fact]
        public async void DeleteAsync_WithValidExistingStory_ReturnsDeletedRowsCount() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var dummyStory = CreateDummyStory(1);
            context.Stories.Add(dummyStory);
            await context.SaveChangesAsync();
            // Act
            var result = await sut.DeleteAsync(dummyStory);
            // Assert
            Assert.IsType<int>(result);
            Assert.True(result > 0);
            Assert.Empty(context.Stories);
        }

        [Fact]
        public async void DeleteAsync_WithNonExistentStory_ThrowsDbUpdateException() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new StoryRepository(context);
            var dummyStory = CreateDummyStory(1);
            // Act
            var exception = await Record.ExceptionAsync(() => sut.DeleteAsync(dummyStory));
            // Assert
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }
    }
}