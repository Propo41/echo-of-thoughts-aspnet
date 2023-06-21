using System.Net;

namespace Cefalo.EchoOfThoughts.Domain.Dto {
    public class ErrorResponse {
        public int StatusCode { get; set; }
        public string Value { get; set; }
        public object Data { get; set; }

        public ErrorResponse() {
            StatusCode = (int) HttpStatusCode.InternalServerError;
            Value = null;
        }

    }
}
