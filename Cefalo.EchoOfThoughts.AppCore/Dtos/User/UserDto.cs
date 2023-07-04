using Cefalo.EchoOfThoughts.AppCore.Dtos.Story;

namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserDto {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int Role { get; set; }
        public ICollection<AuthoredStoryDto> Stories { get; set; }
    }
}
