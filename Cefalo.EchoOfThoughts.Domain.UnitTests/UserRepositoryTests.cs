using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cefalo.EchoOfThoughts.Domain.UnitTests {
    public class UserRepositoryTests : TestConfig {
        public UserRepositoryTests() {
            var dbName = $"UsersDb_{DateTime.Now.ToFileTimeUtc()}";
            InitializeDbContext(dbName);
        }

        [Fact]
        public async void CreateAsync_WithRequiredFields_ReturnsNewUser() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            var user = CreateDummyUser(1, "ali123");

            // Act
            var result = await sut.CreateAsync(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user, result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
        }

        [Fact]
        public async void CreateAsync_WithMissingFields_ThrowsDbException() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            var user = CreateDummyUser(1, "ali123");
            user.UserName = null;

            // Act
            var exception = await Record.ExceptionAsync(() => sut.CreateAsync(user));

            // Assert
            Assert.IsType<DbUpdateException>(exception);
        }


        [Theory]
        [InlineData("ali", 1)]
        [InlineData("bojack", 1)]
        [InlineData("", 4)]
        [InlineData("t", 2)]
        public async void FindAllAsync_HavingPreExistingUsersAndValidUsernames_ReturnsUserWithGivenUsernames(string username, int expectedResultCount) {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            var users = new List<User>
            {
                CreateDummyUser(1, $"ali"),
                CreateDummyUser(2, $"bojack"),
                CreateDummyUser(3, $"hettly"),
                CreateDummyUser(4, $"tryp")
            };
            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            // Act
            IEnumerable<User> result = await sut.FindAllAsync(username);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResultCount, result.Count());
        }

        [Fact]
        public async void FindAsync_WithNonExistentId_ReturnsNull() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);

            // Act
            var result = await sut.FindAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void FindAsync_WithExistingId_ReturnsUser() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            const int authoredStoriesCount = 3;
            var user = CreateDummyUser(1, "ali", authoredStoriesCount);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await sut.FindAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Story>(result.Stories.First());
            Assert.Equal(user, result);
            Assert.Equal(user.Stories.Count, authoredStoriesCount);
        }

        [Fact]
        public async void FindAsync_WithNonExistentUsername_ReturnsNull() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);

            // Act
            var result = await sut.FindAsync("dummy");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void FindAsync_WithExistingUsername_ReturnsUser() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            const int authoredStoriesCount = 3;
            var user = CreateDummyUser(1, "ali", authoredStoriesCount);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await sut.FindAsync("ali");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Story>(result.Stories.First());
            Assert.Equal(user, result);
            Assert.Equal(user.Stories.Count, authoredStoriesCount);
        }

        [Fact]
        public async void FindByEmailAsync_WithExistingEmail_ReturnsUser() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            const int authoredStoriesCount = 3;
            var user = CreateDummyUser(1, "ali", authoredStoriesCount);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await sut.FindByEmailAsync(user.Email);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Story>(result.Stories.First());
            Assert.Equal(user, result);
            Assert.Equal(user.Stories.Count, authoredStoriesCount);
        }

        [Fact]
        public async void FindByEmailAsync_WithNonExistentEmail_ReturnsNull() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);

            // Act
            var result = await sut.FindByEmailAsync("dummyEmail@gmil.com");

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async void UpdateAsync_WithExistingUserAndNecessaryFields_ReturnsUpdatedUser() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            var user = CreateDummyUser(1, "ali");
            context.Users.Add(user);
            await context.SaveChangesAsync();

            user.FullName = "new name";

            // Act
            var result = await sut.UpdateAsync(user);

            // Assert
            Assert.Equal(user, result);
            Assert.Equal(user.FullName, result.FullName);
        }

        [Fact]
        public async void UpdateAsync_WithNonExistingUser_ThrowsDbUpdateException() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            var user = CreateDummyUser(1, "ali");
            user.FullName = "new name";

            // Act
            var exception = await Record.ExceptionAsync(() => sut.UpdateAsync(user));

            // Assert
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }

        [Fact]
        public async void DeleteAsync_WithValidExistingUser_ReturnsDeletedRowsCount() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);

            var dummyUser = CreateDummyUser(1, "ali");
            context.Users.Add(dummyUser);
            await context.SaveChangesAsync();
            // Act
            var result = await sut.DeleteAsync(dummyUser);
            // Assert
            Assert.IsType<int>(result);
            Assert.Empty(context.Stories);
        }

        [Fact]
        public async void DeleteAsync_WithNonExistentStory_ThrowsDbUpdateException() {
            // arrange
            var context = await CreateDbContextAsync();
            var sut = new UserRepository(context);
            var dummyUser = CreateDummyUser(1, "ali");
            // Act
            var exception = await Record.ExceptionAsync(() => sut.DeleteAsync(dummyUser));
            // Assert
            Assert.IsType<DbUpdateConcurrencyException>(exception);
        }
    }
}