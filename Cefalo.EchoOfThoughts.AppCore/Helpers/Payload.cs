namespace Cefalo.EchoOfThoughts.AppCore.Helpers {
    public class Payload {
        public string Message { get; set; }
        public string Data { get; set; }
        public Payload() { }

        public Payload(string message) {
            this.Message = message;
        }

        public Payload(string message, string data) {
            this.Data = data;
            this.Message = message;
        }

    }
}
