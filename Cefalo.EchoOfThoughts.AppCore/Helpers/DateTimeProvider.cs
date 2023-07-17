using Cefalo.EchoOfThoughts.AppCore.Helpers.Interfaces;

namespace Cefalo.EchoOfThoughts.AppCore.Helpers {

    public class DateTimeProvider : IDateTimeProvider {
        public DateTime GetCurrentTime() {
            return DateTime.Now.ToUniversalTime();
        }
    }
}
