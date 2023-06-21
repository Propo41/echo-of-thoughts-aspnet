namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public class Payload {
        public string Message { get; set; }
        public Payload() { }

        public Payload(string message) {
            this.Message = message;
        }

    }
}
