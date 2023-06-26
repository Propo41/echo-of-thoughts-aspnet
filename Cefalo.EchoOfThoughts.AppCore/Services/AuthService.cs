using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class AuthService : IAuthService {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration) {
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<Tuple<UserDto, string>> Create(UserSignUpDto userDto) {
            // check if user with the email already exists
            var userByEmail = await _userRepository.FindByEmail(userDto.Email);
            if (userByEmail != null) {
                throw new BadRequestException("An account with the email already exists!");
            }

            var userByUsername = await _userRepository.FindByUsername(userDto.UserName);
            if (userByUsername != null) {
                throw new BadRequestException("An account with the username already exists!");
            }
            // hash password and save in db
            var hashPassword = Auth.HashPassword(userDto.Password);
            var userEntity = _mapper.Map<User>(userDto);
            userEntity.PasswordHash = hashPassword;
            var newUser = await _userRepository.CreateAsync(userEntity);

            // create a jwt string
            var jwtString = Auth.CreateJwt(newUser, _configuration);
            var newUserDto = _mapper.Map<UserDto>(newUser);

            return new Tuple<UserDto, string>(newUserDto, jwtString);
        }

        public async Task<string> SignIn(UserSignInDto userSignDto) {
            // check if email exists
            var user = await _userRepository.FindByEmail(userSignDto.Email);
            if (user == null) {
                throw new NotFoundException("An account with the email does not exist!");
            }
            // check if password valid
            var isValid = Auth.IsPasswordValid(userSignDto.Password, user.PasswordHash);
            if (!isValid) {
                throw new BadRequestException("Incorrect password entered!");
            }
            return Auth.CreateJwt(user, _configuration);
        }

        public Task<UserDto> FindByEmail(string email) {
            throw new NotImplementedException();
        }

        public Task<Payload> Delete(int id) {
            throw new NotImplementedException();
        }

        public async Task<Payload> UpdatePassword(int userId, UserPasswordDto passwordDto) {
            var existingUser = await _userRepository.FindById(userId);
            if (existingUser == null) {
                throw new NotFoundException("User not found");
            }
            if (!passwordDto.ConfirmPassword.Equals(passwordDto.NewPassword)) {
                throw new BadRequestException("Password's do not match");
            }
            var isPasswordValid = Auth.IsPasswordValid(passwordDto.OldPassword, existingUser.PasswordHash);
            if (!isPasswordValid) {
                throw new BadRequestException("Password is incorrect");
            }

            var hashPassword = Auth.HashPassword(passwordDto.NewPassword);
            existingUser.PasswordHash = hashPassword;
            await _userRepository.Update(existingUser);
            return new Payload("Password updated successfully");
        }

        public Task<Payload> SignOut() {
            throw new NotImplementedException();
        }
    }
}
