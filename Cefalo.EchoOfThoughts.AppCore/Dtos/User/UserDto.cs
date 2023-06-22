﻿namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserDto {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public int Role { get; set; }
        public ICollection<Domain.Entities.Story> Stories { get; set; }
    }
}
