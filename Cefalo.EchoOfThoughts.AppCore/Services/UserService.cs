using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Cefalo.EchoOfThoughts.Domain.Repositories.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(string username) {
            var users = await _userRepository.FindAllAsync(username);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> FindAsync(int id) {
            var existingUser = await _userRepository.FindAsync(id);
            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<UserDto> FindAsync(string username) {
            var existingUser = await _userRepository.FindAsync(username);
            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<UserUpdateDto> UpdateAsync(int id, UserUpdateDto updateDto) {
            var existingUser = await _userRepository.FindAsync(id);
            if (existingUser == null) {
                throw new NotFoundException("User not found. Try logging in again");
            }

            updateDto.MapToModel(existingUser);
            var res = await _userRepository.UpdateAsync(existingUser);
            return _mapper.Map<UserUpdateDto>(res);
        }

        public async Task<Payload> DeleteByIdAsync(int id) {
            var existingUser = await _userRepository.FindAsync(id);
            if (existingUser == null) {
                throw new NotFoundException("User not found");
            }
            await _userRepository.DeleteAsync(existingUser);
            return new Payload("User deleted successfully");
        }
    }
}
