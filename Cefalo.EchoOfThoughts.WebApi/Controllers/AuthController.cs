using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;
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
        public async Task<UserDto> Register(UserSignUpDto signUpDto) {
            var (userDto, jwtString) = await _authService.Create(signUpDto);

            Response.Cookies.Append("session-token", jwtString, new CookieOptions {
                Expires = DateTimeOffset.Now.AddDays(1),
                HttpOnly = true
            });

            return userDto;
        }

        // POST api/auth/sign-in
        [HttpPost("sign-in")]
        public async Task<Payload> LogIn(UserSignInDto signInDto) {
            var jwtString = await _authService.SignIn(signInDto);

            Response.Cookies.Append("session-token", jwtString, new CookieOptions {
                Expires = DateTimeOffset.Now.AddDays(1),
                HttpOnly = true
            });

            return new Payload("Sign in successful!");
        }

        // POST api/auth/sign-out
        [HttpPost("sign-out")]
        [Authorize]
        public Payload LogOut() {
            if (Request.Cookies["session-token"] != null) {
                Response.Cookies.Delete("session-token");
                return new Payload("Signed out!");
            }

            return new Payload("Already signed out!");
        }

        // PUT api/auth/change-password
        [HttpPut("change-password")]
        [Authorize]
        public async Task<Payload> UpdatePasswordAsync([FromBody] UserPasswordDto passwordDto) {
            var id = HttpContext.User.FindFirst("Id");
            return await _authService.UpdatePassword(int.Parse(id!.Value), passwordDto);
        }

    }
}
