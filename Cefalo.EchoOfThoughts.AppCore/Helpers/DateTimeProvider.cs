namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public interface IDateTimeProvider {
        DateTime GetCurrentTime();
    }
    public class DateTimeProvider : IDateTimeProvider {
        public DateTime GetCurrentTime() {
            return DateTime.Now.ToUniversalTime();
        }
    }
}
