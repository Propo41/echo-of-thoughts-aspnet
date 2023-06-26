using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            var user = await _userService.FindById(id);
            return user;
        }

        // PUT api/users
        [HttpPut]
        [Authorize]
        public async Task<UserUpdateDto> UpdateAsync([FromBody] UserUpdateDto updateDto) {
            if (updateDto == null) {
                throw new BadRequestException("No body provided for update");
            }
            var id = HttpContext.User.FindFirst("Id");
            return await _userService.Update(int.Parse(id!.Value), updateDto);
        }

        // DELETE api/users
        [HttpDelete]
        [Authorize]
        public async Task<Payload> DeleteAsync() {
            var id = HttpContext.User.FindFirst("Id");
            return await _userService.DeleteById(int.Parse(id!.Value));
        }
    }
}
