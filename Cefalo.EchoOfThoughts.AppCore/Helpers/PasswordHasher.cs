namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public static class PasswordHasher {
        public static string HashPassword(string password) {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool isValid(string password, string hash) {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
