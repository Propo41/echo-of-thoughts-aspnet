﻿using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
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

        // GET api/users?username=ali
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllAsync([FromQuery] string? username) {
            _logger.LogInformation("fetching all users");
            return await _userService.GetAll(username);
        }

        // GET api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<UserDto> Get(int id) {
            _logger.LogInformation("fetching single user with id: {id}", id);
            var user = await _userService.Find(id);
            return user;
        }

        // GET api/users/{username}
        [HttpGet("{username:alpha}")]
        public async Task<UserDto> GetByUsername(string username) {
            _logger.LogInformation("fetching user by username: {username}", username);
            var user = await _userService.Find(username);
            return user;
        }

        // PUT api/users
        [HttpPut]
        [Authorize]
        public async Task<UserUpdateDto> UpdateAsync([FromBody] UserUpdateDto updateDto) {
            if (updateDto == null) {
                throw new BadRequestException("No body provided for update");
            }
            var id = HttpContext.User.FindFirst("Id")?.Value;
            _logger.LogInformation("updating user with id: {id} with contents {user}", id, updateDto);
            return await _userService.Update(int.Parse(id!), updateDto);
        }

        // DELETE api/users
        [HttpDelete]
        [Authorize]
        public async Task<Payload> DeleteAsync() {
            var id = HttpContext.User.FindFirst("Id")?.Value;
            _logger.LogInformation("Deleting user with id:{id}", id);
            return await _userService.DeleteById(int.Parse(id!));
        }
    }
}
