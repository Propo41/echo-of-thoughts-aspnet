using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Entities;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Create(UserSignUpDto signUpDto) {
            var hashPassword = Auth.HashPassword(signUpDto.Password);
            var user = _mapper.Map<User>(signUpDto);
            user.PasswordHash = hashPassword;

            var res = await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(res);
        }

        public async Task<IEnumerable<UserDto>> GetAll() {
            var users = await _userRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> Find(int id) {
            var existingUser = await _userRepository.Find(id);
            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<UserDto> Find(string username) {
            var existingUser = await _userRepository.Find(username);
            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<UserUpdateDto> Update(int id, UserUpdateDto updateDto) {
            var existingUser = await _userRepository.Find(id);
            if (existingUser == null) {
                throw new NotFoundException("User not found. Try logging in again");
            }

            updateDto.MapToModel(existingUser);
            var res = await _userRepository.Update(existingUser);
            return _mapper.Map<UserUpdateDto>(res);
        }

        public async Task<Payload> DeleteById(int id) {
            var existingUser = await _userRepository.Find(id);
            if (existingUser == null) {
                throw new NotFoundException("User not found");
            }
            await _userRepository.DeleteAsync(existingUser);
            return new Payload("User deleted successfully");
        }
    }
}
