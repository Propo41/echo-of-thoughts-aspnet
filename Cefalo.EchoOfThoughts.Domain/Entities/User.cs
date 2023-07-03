namespace Cefalo.EchoOfThoughts.Domain.Entities {
    public class User {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime PasswordUpdatedAt { get; set; }
        public int Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Story> Stories { get; set; } = new List<Story>();
    }
}
