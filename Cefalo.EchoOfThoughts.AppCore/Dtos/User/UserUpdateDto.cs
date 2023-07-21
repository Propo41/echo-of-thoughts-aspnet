using System.ComponentModel.DataAnnotations;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserUpdateDto {
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 3)]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Url]
        public string ProfilePicture { get; set; }
        public void MapToModel(Domain.Entities.User user) {
            user.FullName = FullName;
            user.Email = Email;
            user.ProfilePicture = ProfilePicture;
        }
    }
}
