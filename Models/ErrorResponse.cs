using System.Net;

namespace Cefalo.EchoOfThoughts.Domain
{
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Value { get; set; }
        public object? Data { get; set; }

    }
}
