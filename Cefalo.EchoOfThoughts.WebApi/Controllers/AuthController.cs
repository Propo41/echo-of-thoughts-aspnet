using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cefalo.EchoOfThoughts.WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly ILogger<StoriesController> _logger;
        public AuthController(IAuthService authService, ILogger<StoriesController> logger) {
            _authService = authService;
            _logger = logger;
        }

        // POST api/auth/sign-up
        [HttpPost("sign-up")]
        public async Task<UserDto> RegisterAsync(UserSignUpDto signUpDto) {
            _logger.LogInformation("creating new user: {user}", signUpDto);
            var userDto = await _authService.CreateAsync(signUpDto);
            return userDto;
        }

        // POST api/auth/sign-in
        [HttpPost("sign-in")]
        public async Task<Payload> LogInAsync(UserSignInDto signInDto) {
            var jwtString = await _authService.SignInAsync(signInDto);
            _logger.LogInformation("signing in, user: {user}", signInDto);
            //Response.Cookies.Append("session-token", jwtString, new CookieOptions {
            //    Expires = DateTimeOffset.Now.AddDays(1),
            //    HttpOnly = false,
            //    Secure = false
            //});
            return new Payload("Sign in successful!", jwtString);
        }

        // POST api/auth/sign-out
        [HttpPost("sign-out")]
        [Authorize]
        public Payload LogOut() {
            //if (Request.Cookies["session-token"] != null) {
            //    Response.Cookies.Delete("session-token");
            //    return new Payload("Signed out!");
            //}
            return new Payload("Already signed out!");
        }

        // PUT api/auth/change-password
        [HttpPut("change-password")]
        [Authorize]
        public async Task<Payload> UpdatePasswordAsync([FromBody] UserPasswordDto passwordDto) {
            var id = HttpContext.User.FindFirst("Id")?.Value;
            _logger.LogInformation("change password request from user id: {id}", id);
            return await _authService.UpdatePasswordAsync(int.Parse(id!), passwordDto);
        }

    }
}
