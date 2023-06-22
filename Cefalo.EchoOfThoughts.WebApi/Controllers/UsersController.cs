using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.EchoOfThoughts.WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService userService, ILogger<UsersController> logger) {
            _userService = userService;
            _logger = logger;
        }

        // GET api/users
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllAsync() {
            return await _userService.GetAll();
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<UserDto> Get(int id) {
            var story = await _userService.FindById(id);
            return story;
        }

        // POST api/users
        [HttpPost]
        public async Task<UserDto> PostAsync([FromBody] UserSignUpDto signUpDto) {
            return await _userService.Create(signUpDto);
        }

        // PUT api/users
        [HttpPut]
        public async Task<UserDto> UpdateAsync([FromBody] UserUpdateDto updateDto) {
            if (updateDto == null) {
                throw new BadRequestException("No body provided for update");
            }

            var id = 4; // todo: fetch this from httpContext after authentication added
            return await _userService.Update(id, updateDto);
        }

        // DELETE api/users/{id}
        [HttpDelete("{id:int}")]
        public async Task<Payload> DeleteAsync(int id) {
            return await _userService.DeleteById(id);
        }

    }
}
