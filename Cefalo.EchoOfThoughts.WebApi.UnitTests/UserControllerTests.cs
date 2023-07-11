using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.WebApi.Controllers;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NuGet.ContentModel;

namespace Cefalo.EchoOfThoughts.WebApi.UnitTests {
    public class UserControllerTests {
        private readonly IUserService _userService;
        private readonly UsersController _sut;
        public UserControllerTests() {
            _userService = A.Fake<IUserService>();
            var logger = A.Fake<ILogger<UsersController>>();
            _sut = new UsersController(_userService, logger);
        }

        [Fact]
        public async void GetAllAsync_NoUsers_ReturnsEmptyList() {
            // arrange
            var expectedUsers = new List<UserDto>();
            const string username = "dummy";
            A.CallTo(() => _userService.GetAllAsync(username)).Returns(expectedUsers);

            // act
            var result = await _sut.GetAllAsync(username);

            // assert
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllAsync_HasUsers_ReturnsPopulatedList() {
            // arrange
            var expectedUsers = new List<UserDto>
            {
                new(){Id=1}, new(){Id=2}, new(){Id=3}, new(){Id=4}
            };
            const string username = "";
            A.CallTo(() => _userService.GetAllAsync(username)).Returns(expectedUsers);

            // act
            var result = await _sut.GetAllAsync(username);

            // assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(expectedUsers.Count, result.Count());
        }

        [Fact]
        public async void GetAsync_HasUser_ReturnsUser() {
            // arrange
            var expectedUser = new UserDto {
                Id = 1
            };
            var id = 1;
            A.CallTo(() => _userService.FindAsync(id)).Returns(expectedUser);

            // act
            var result = await _sut.GetAsync(id);

            // assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async void GetByUsernameAsync_HasUser_ReturnsUser() {
            // arrange
            var expectedUser = new UserDto {
                Id = 1,
                UserName = "broodmother"
            };
            var username = "broodmother";
            A.CallTo(() => _userService.FindAsync(username)).Returns(expectedUser);

            // act
            var result = await _sut.GetByUsernameAsync(username);

            // assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser, result);
        }

        [Fact]
        public async void UpdateAsync_EmptyBody_ThrowsBadRequestException() {
            // arrange
            UserUpdateDto? updateDto = null;
            // act & assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await _sut.UpdateAsync(updateDto));
        }

    }
}