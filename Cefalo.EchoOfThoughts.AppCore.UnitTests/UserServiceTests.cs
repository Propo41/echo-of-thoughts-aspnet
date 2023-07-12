using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using FakeItEasy;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class UserServiceTests {
        private readonly IUserRepository _userRepository;
        private readonly UserService _sut;

        public UserServiceTests() {
            _userRepository = A.Fake<IUserRepository>();
            // mapper config
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<StoryMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            _sut = new UserService(_userRepository, mapper);

        }

        [Fact]
        public async void GetAllAsync_NoUsers_ReturnsEmptyList() {
            // arrange
            var expectedUsers = new List<User>();
            const string username = "dummy";
            A.CallTo(() => _userRepository.FindAllAsync(username)).Returns(expectedUsers);

            // act
            var result = await _sut.GetAllAsync(username);

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllAsync_HasUsers_ReturnsUserList() {
            // arrange
            var expectedUsers = new List<User> {
                new() { UserName = "ali" },
                new() { UserName = "ahnaf" }
            };

            const string username = "";
            A.CallTo(() => _userRepository.FindAllAsync(username)).Returns(expectedUsers);

            // act
            var result = await _sut.GetAllAsync(username);

            // assert
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
            Assert.Equal(result.Count(), expectedUsers.Count);
        }

        [Fact]
        public async void FindAsync_HasUser_ReturnsUser() {
            // arrange
            var expectedUser = new User {
                UserName = "ali",
                Id = 1
            };

            const int id = 1;
            A.CallTo(() => _userRepository.FindAsync(id)).Returns(expectedUser);

            // act
            var result = await _sut.FindAsync(id);

            // assert
            Assert.IsAssignableFrom<UserDto>(result);
            Assert.Equal(result.UserName, expectedUser.UserName);
            Assert.Equal(result.Id, expectedUser.Id);
        }


        [Fact]
        public async void UpdateAsync_NoBody_ThrowsNotFoundException() {
            // arrange
            const int id = 1;
            User? existingUser = null;
            UserUpdateDto? updateDto = null;
            A.CallTo(() => _userRepository.FindAsync(id)).Returns(existingUser);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(
                async () => await _sut.UpdateAsync(id, updateDto));
        }

        [Fact]
        public async void UpdateAsync_HasBody_ReturnsUpdatedUser() {
            // arrange
            const int id = 1;
            var existingUser = new User {
                Id = 1,
                Email = "aliahnaf@gmail.com"
            };
            var updateDto = new UserUpdateDto {
                Email = "ali@gmail.com"
            };

            var updatedUser = new User {
                Id = existingUser.Id,
                Email = updateDto.Email
            };

            A.CallTo(() => _userRepository.FindAsync(id)).Returns(existingUser);
            A.CallTo(() => _userRepository.UpdateAsync(existingUser)).Returns(updatedUser);

            // act
            var result = await _sut.UpdateAsync(id, updateDto);

            // assert
            Assert.IsAssignableFrom<UserUpdateDto>(result);
            Assert.Equal(existingUser.Id, updatedUser.Id);
        }

        [Fact]
        public async void DeleteById_InvalidId_ThrowsNotFoundException() {
            // arrange
            const int id = 1;
            User? existingUser = null;
            A.CallTo(() => _userRepository.FindAsync(id)).Returns(existingUser);

            // act & assert
            await Assert.ThrowsAsync<NotFoundException>(
                async () => await _sut.DeleteByIdAsync(id));

        }

        [Fact]
        public async void DeleteById_HasProperId_ReturnsPayload() {
            // arrange
            const int id = 1;
            var existingUser = new User {
                Id = 1,
                Email = "aliahnaf@gmail.com"
            };
            A.CallTo(() => _userRepository.FindAsync(id)).Returns(existingUser);
            A.CallTo(() => _userRepository.DeleteAsync(existingUser)).Returns(1);

            // act
            var result = await _sut.DeleteByIdAsync(id);

            // assert
            Assert.IsAssignableFrom<Payload>(result);
            Assert.NotNull(result);
        }
    }
}