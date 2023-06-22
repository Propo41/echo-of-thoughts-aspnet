using AutoMapper;
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
            var hashPassword = PasswordHasher.HashPassword(signUpDto.Password);
            var user = _mapper.Map<User>(signUpDto);
            user.PasswordHash = hashPassword;

            var res = await _userRepository.CreateAsync(user);
            return _mapper.Map<UserDto>(res);

        }

        public Task<IEnumerable<UserDto>> GetAll() {
            throw new NotImplementedException();
        }

        public Task<UserDto> FindById(int id) {
            throw new NotImplementedException();
        }

        public async Task<UserDto> Update(int id, UserUpdateDto updateDto) {
            var existingUser = await _userRepository.FindById(id);
            if (existingUser == null) {
                throw new NotFoundException("User not found. Try logging in again");
            }
            var user = _mapper.Map<User>(updateDto);
            user.Id = id;
            var res = await _userRepository.Update(user);
            return _mapper.Map<UserDto>(res);
        }

        public Task<Payload> DeleteById(int id) {
            throw new NotImplementedException();
        }

        public Task<Payload> UpdateRole(int id, string[] roles) {
            throw new NotImplementedException();
        }
    }
}
