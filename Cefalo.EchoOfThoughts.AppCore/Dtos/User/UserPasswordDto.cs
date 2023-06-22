namespace Cefalo.EchoOfThoughts.AppCore.Dtos.User {
    public class UserPasswordDto {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
