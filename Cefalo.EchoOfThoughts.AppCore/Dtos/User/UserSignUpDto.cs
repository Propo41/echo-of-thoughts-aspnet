using System.ComponentModel.DataAnnotations;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserSignUpDto {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Url]
        public string ProfilePicture { get; set; }
    }
}
