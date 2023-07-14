using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Cefalo.EchoOfThoughts.AppCore.Services;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Configuration;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class AuthServiceTests : TestConfig {
        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAuthService _sut;
        private readonly IConfiguration _configuration;
        private readonly IAuthHelper _authHelper;

        public AuthServiceTests() {
            _userRepository = A.Fake<IUserRepository>();
            _dateTimeProvider = A.Fake<IDateTimeProvider>();
            _configuration = A.Fake<IConfiguration>();
            _authHelper = A.Fake<IAuthHelper>();
            _sut = new AuthService(_userRepository, Mapper, _configuration, _dateTimeProvider, _authHelper);
        }

        [Fact]
        public async void CreateAsync_WithCorrectFields_ReturnsNewUser() {
            // arrange
            var userDto = SignUpDtoLists.FirstOrDefault();

            User? existingUser = null;
            const string hashedPassword = "this is a hash";
            var createdTime = DateTime.Today;

            var user = new User {
                Email = userDto!.Email,
            };

            A.CallTo(() => _userRepository.FindByEmailAsync(userDto.Email)).Returns(existingUser);
            A.CallTo(() => _userRepository.FindAsync(userDto.UserName)).Returns(existingUser);
            A.CallTo(() => _authHelper.HashPassword(userDto.Password)).Returns(hashedPassword);
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).Returns(createdTime);
            A.CallTo(() => _userRepository.CreateAsync(A<User>._)).Returns(user);

            // act
            var result = await _sut.CreateAsync(userDto);

            // assert
            Assert.IsType<UserDto>(result);
            Assert.Equal(result.Email, user.Email);
            Assert.Equal(result.UserName, user.UserName);
            Assert.Empty(result.Stories);

            A.CallTo(() => _userRepository.FindByEmailAsync(userDto.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.FindAsync(userDto.UserName)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.HashPassword(userDto.Password)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.CreateAsync(A<User>.That.Matches(x => x.Email == userDto.Email))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void CreateAsync_WithEmailAlreadyExisting_ThrowsBadRequestException() {
            // arrange
            var userDto = SignUpDtoLists.FirstOrDefault();
            var existingUser = new User {
                Email = userDto!.Email,
                UserName = userDto.UserName,
            };
            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>._)).Returns(existingUser);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.CreateAsync(userDto));

            // assert
            Assert.IsType<BadRequestException>(exception);
            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>.That.Matches(x => x == userDto.Email))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void CreateAsync_WithUsernameAlreadyExisting_ThrowsBadRequestException() {
            // arrange
            var userDto = SignUpDtoLists.FirstOrDefault();
            User? existingUserByEmail = null;
            var existingUserByUsername = new User {
                UserName = userDto!.UserName
            };
            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>._)).Returns(existingUserByEmail);
            A.CallTo(() => _userRepository.FindAsync(A<string>._)).Returns(existingUserByUsername);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.CreateAsync(userDto));

            // assert
            Assert.IsType<BadRequestException>(exception);
            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>.That.Matches(x => x == userDto.Email))).MustHaveHappenedOnceExactly();
            A.CallTo(() => _userRepository.FindAsync(A<string>.That.Matches(x => x == userDto.UserName))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignInAsync_WithCorrectFields_ReturnsJwt() {
            // arrange
            var userDto = SignInDtoLists.FirstOrDefault();
            const string passwordHash = "hashPassword";
            var existingUser = new User {
                Email = userDto!.Email,
                PasswordHash = passwordHash
            };
            const string fakeJwt = "fake jwt";

            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>._)).Returns(existingUser);
            A.CallTo(() => _authHelper.IsPasswordValid(A<string>._, A<string>._)).Returns(true);
            A.CallTo(() => _authHelper.CreateJwt(A<User>._, _configuration)).Returns(fakeJwt);

            // act
            var result = await _sut.SignInAsync(userDto);

            // assert
            Assert.IsType<string>(result);
            Assert.Equal(result, fakeJwt);

            A.CallTo(() => _userRepository.FindByEmailAsync(userDto.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.IsPasswordValid(userDto.Password, existingUser.PasswordHash)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.CreateJwt(A<User>.That.Matches(x => x.Email == userDto.Email), _configuration))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignInAsync_WithNonExistentEmail_ThrowsNotFoundException() {
            // arrange
            var userDto = SignInDtoLists.FirstOrDefault();
            User? existingUser = null;
            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>._)).Returns(existingUser);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(userDto));

            // assert
            Assert.IsType<NotFoundException>(exception);
            A.CallTo(() => _userRepository.FindByEmailAsync(userDto!.Email)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void SignInAsync_WithIncorrectPassword_BadRequestException() {
            // arrange
            var userDto = SignInDtoLists.FirstOrDefault();
            var existingUser = new User {
                Email = userDto!.Email,
                PasswordHash = "hashedPassword"
            };

            A.CallTo(() => _userRepository.FindByEmailAsync(A<string>._)).Returns(existingUser);
            A.CallTo(() => _authHelper.IsPasswordValid(A<string>._, A<string>._)).Returns(false);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.SignInAsync(userDto));

            // assert
            Assert.IsType<BadRequestException>(exception);
            A.CallTo(() => _userRepository.FindByEmailAsync(userDto.Email)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.IsPasswordValid(userDto.Password, existingUser.PasswordHash)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdatePasswordAsync_WithCorrectFields_ReturnsPayload() {
            // arrange
            var userDto = new UserPasswordDto() {
                OldPassword = "oldPassword",
                NewPassword = "newPassword",
                ConfirmPassword = "newPassword"
            };
            const int userId = 1;
            const string oldPasswordHash = "hashedOldPassword";
            const string passwordHash = "hashedNewPassword";
            var existingUser = new User {
                Email = "ali@gmail.com",
                PasswordHash = oldPasswordHash
            };
            var updatedTime = DateTime.Today;

            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);
            A.CallTo(() => _authHelper.IsPasswordValid(A<string>._, A<string>._)).Returns(true);
            A.CallTo(() => _authHelper.HashPassword(A<string>._)).Returns(passwordHash);
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).Returns(updatedTime);

            // act
            var result = await _sut.UpdatePasswordAsync(userId, userDto);

            // assert
            Assert.IsType<Payload>(result);
            A.CallTo(() => _userRepository.FindAsync(userId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.IsPasswordValid(userDto.OldPassword, A<string>.That.Matches(x => x == oldPasswordHash)))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.HashPassword(userDto.NewPassword)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _dateTimeProvider.GetCurrentTime()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdatePasswordAsync_WithNoExistingUser_ThrowsNotFoundException() {
            // arrange
            const int userId = 1;
            var userDto = UserPasswordLists.FirstOrDefault();
            User? existingUser = null;
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.UpdatePasswordAsync(userId, userDto));

            // assert
            Assert.IsType<NotFoundException>(exception);
            A.CallTo(() => _userRepository.FindAsync(userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdatePasswordAsync_PasswordsMismatch_ThrowsBadRequestException() {
            // arrange
            const int userId = 1;
            var userDto = UserPasswordLists.FirstOrDefault();
            userDto!.NewPassword = "mismatchedPassword";
            var existingUser = Users.FirstOrDefault();
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.UpdatePasswordAsync(userId, userDto));

            // assert
            Assert.IsType<BadRequestException>(exception);
            A.CallTo(() => _userRepository.FindAsync(userId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void UpdatePasswordAsync_PasswordIncorrect_ThrowsBadRequestException() {
            // arrange
            const int userId = 1;
            var userDto = UserPasswordLists.FirstOrDefault();
            var existingUser = Users.FirstOrDefault();
            A.CallTo(() => _userRepository.FindAsync(A<int>._)).Returns(existingUser);
            A.CallTo(() => _authHelper.IsPasswordValid(A<string>._, A<string>._)).Returns(false);

            // act
            var exception = await Record.ExceptionAsync(() => _sut.UpdatePasswordAsync(userId, userDto));

            // assert
            Assert.IsType<BadRequestException>(exception);
            A.CallTo(() => _userRepository.FindAsync(userId)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _authHelper.IsPasswordValid(userDto!.OldPassword, existingUser!.PasswordHash)).MustHaveHappenedOnceExactly();
        }
    }
}