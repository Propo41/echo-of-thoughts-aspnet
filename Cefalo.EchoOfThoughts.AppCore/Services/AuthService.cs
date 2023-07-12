using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class AuthService : IAuthService {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAuthHelper _authHelper;
        public AuthService(IUserRepository userRepository, IMapper mapper,
            IConfiguration configuration, IDateTimeProvider dateTimeProvider, IAuthHelper authHelper) {
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _authHelper = authHelper;
        }
        public async Task<UserDto> CreateAsync(UserSignUpDto userDto) {
            // check if user with the email already exists
            var userByEmail = await _userRepository.FindByEmailAsync(userDto.Email);
            if (userByEmail != null) {
                throw new BadRequestException("An account with the email already exists!");
            }

            var userByUsername = await _userRepository.FindAsync(userDto.UserName);
            if (userByUsername != null) {
                throw new BadRequestException("An account with the username already exists!");
            }
            // hash password and save in db
            var hashPassword = _authHelper.HashPassword(userDto.Password);

            var userEntity = _mapper.Map<User>(userDto);
            userEntity.PasswordHash = hashPassword;
            var currentTime = _dateTimeProvider.GetCurrentTime();
            userEntity.CreatedAt = currentTime;
            userEntity.PasswordUpdatedAt = currentTime;

            var newUser = await _userRepository.CreateAsync(userEntity);
            var newUserDto = _mapper.Map<UserDto>(newUser);

            return newUserDto;
        }

        public async Task<string> SignInAsync(UserSignInDto userSignDto) {
            // check if email exists
            var user = await _userRepository.FindByEmailAsync(userSignDto.Email);
            if (user == null) {
                throw new NotFoundException("An account with the email does not exist!");
            }
            // check if password valid
            var isValid = _authHelper.IsPasswordValid(userSignDto.Password, user.PasswordHash);
            if (!isValid) {
                throw new BadRequestException("Incorrect password entered!");
            }
            return _authHelper.CreateJwt(user, _configuration);
        }

        public async Task<Payload> UpdatePasswordAsync(int userId, UserPasswordDto passwordDto) {
            var existingUser = await _userRepository.FindAsync(userId);
            if (existingUser == null) {
                throw new NotFoundException("User not found");
            }
            if (!passwordDto.ConfirmPassword.Equals(passwordDto.NewPassword)) {
                throw new BadRequestException("Password's do not match");
            }
            var isPasswordValid = _authHelper.IsPasswordValid(passwordDto.OldPassword, existingUser.PasswordHash);
            if (!isPasswordValid) {
                throw new BadRequestException("Password is incorrect");
            }

            var hashPassword = _authHelper.HashPassword(passwordDto.NewPassword);
            var currentTime = _dateTimeProvider.GetCurrentTime();
            existingUser.PasswordHash = hashPassword;
            existingUser.PasswordUpdatedAt = currentTime;

            await _userRepository.UpdateAsync(existingUser);
            return new Payload("Password updated successfully");
        }

    }
}
