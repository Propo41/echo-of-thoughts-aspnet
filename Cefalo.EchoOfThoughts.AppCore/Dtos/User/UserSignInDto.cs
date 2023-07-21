using System.ComponentModel.DataAnnotations;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserSignInDto {
        [Required]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
