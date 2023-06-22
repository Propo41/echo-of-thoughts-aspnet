﻿namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserUpdateDto {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; }
        public void MapToModel(Domain.Entities.User user) {
            user.FullName = FullName;
            user.Email = Email;
            user.ProfilePicture = ProfilePicture;
        }
    }
}