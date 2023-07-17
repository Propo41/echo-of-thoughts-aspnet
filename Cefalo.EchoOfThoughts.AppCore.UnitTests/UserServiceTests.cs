using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using FakeItEasy;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class UserServiceTests : TestConfig {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _sut;

        public UserServiceTests() {
            _userRepository = A.Fake<IUserRepository>();
            _sut = new UserService(_userRepository, Mapper);
        }

        [Fact]
        public async void GetAllAsync_HaveNoUsers_ReturnsEmptyList() {
            // arrange
            const string username = "dummy";
            A.CallTo(() => _userRepository.FindAllAsync(A<string>._)).Returns(new List<User>());

            // act
            var result = await _sut.GetAllAsync(username);

            // assert
            Assert.Empty(result);
            A.CallTo(() => _userRepository.FindAllAsync(username)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void GetAllAsync_HasUsers_ReturnsUserList() {
            // arrange
            const string username = "";
            A.CallTo(() => _userRepository.FindAllAsync(A<string>._)).Returns(Users);

            // act
            var result = await _sut.GetAllAsync(username);

            // assert
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
            Assert.Equal(result.Count(), Users.Count());
            A.CallTo(() => _userRepository.FindAllAsync(username)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void FindAsync_WithExistingId_ReturnsUser() {
            // arrange
            const int id = 1;
            var expectedUser = Users.FirstOrDefault(x => x.Id == id);
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(expectedUser);

            // act
            var result = await _sut.FindAsync(id);

            // assert
            Assert.IsAssignableFrom<UserDto>(result);
            Assert.Equal(result.UserName, expectedUser!.UserName);
            Assert.Equal(result.Id, expectedUser.Id);
            A.CallTo(() => _userRepository.FindAsync(id)).MustHaveHappenedOnceExactly();
        }


        [Fact]
        public async void UpdateAsync_WithNoBody_ThrowsNotFoundException() {
            // arrange
            const int id = 1;
            User? existingUser = null;
            UserUpdateDto? updateDto = null;
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.UpdateAsync(id, updateDto));

            // assert
            Assert.IsType<NotFoundException>(exception);
            A.CallTo(() => _userRepository.FindAsync(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdateAsync_HasBody_ReturnsUpdatedUser() {
            // arrange
            const int id = 1;
            var existingUser = Users.FirstOrDefault();
            var updateDto = new UserUpdateDto {
                Email = "ali@gmail.com"
            };

            var updatedUser = new User {
                Id = existingUser!.Id,
                Email = updateDto.Email
            };

            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);
            A.CallTo(() => _userRepository.UpdateAsync(A<User>._)).Returns(updatedUser);

            // act
            var result = await _sut.UpdateAsync(id, updateDto);

            // assert
            Assert.IsAssignableFrom<UserUpdateDto>(result);
            Assert.Equal(existingUser.Id, updatedUser.Id);
            A.CallTo(() => _userRepository.FindAsync(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.UpdateAsync(existingUser)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void DeleteById_NonExistentStoryId_ThrowsNotFoundException() {
            // arrange
            const int id = 1;
            User? existingUser = null;
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.DeleteByIdAsync(id));

            // assert
            Assert.IsType<NotFoundException>(exception);
            A.CallTo(() => _userRepository.FindAsync(id)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void DeleteById_WithExistingStoryIdAndCorrectAuthorId_ReturnsPayload() {
            // arrange
            const int id = 1;
            var existingUser = Users.FirstOrDefault(x => x.Id == id);
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);
            A.CallTo(() => _userRepository.DeleteAsync(A<User>._)).Returns(1);

            // act
            var result = await _sut.DeleteByIdAsync(id);

            // assert
            Assert.IsType<Payload>(result);
            A.CallTo(() => _userRepository.FindAsync(id)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.DeleteAsync(existingUser)).MustHaveHappenedOnceExactly();
        }
    }
}