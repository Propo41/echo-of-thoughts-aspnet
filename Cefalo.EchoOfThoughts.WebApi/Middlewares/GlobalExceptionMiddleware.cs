using Cefalo.EchoOfThoughts.Domain;
using System.Net;
using System.Text.Json;
using System.Xml.Serialization;

namespace Cefalo.EchoOfThoughts.WebApi.Middlewares
{
    public class GlobalExceptionMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware (RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        /**
         * entry point
         * called in each HTTP request and handles any exception
         */
        public async Task InvokeAsync (HttpContext httpContext) {
            try {
                await _next(httpContext);
            } catch (Exception ex) {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync (HttpContext context, Exception exception) {
            var response = context.Response;

            var errorResponse = new ErrorResponse {
                StatusCode = HttpStatusCode.InternalServerError,
                Value = exception.Message,
                Data = exception.StackTrace
            };

            switch (exception) {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid Token")) {
                        response.StatusCode = (int) HttpStatusCode.Forbidden;
                        errorResponse.Value = ex.Message;
                        break;
                    }
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    errorResponse.Value = ex.Message;
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    errorResponse.Value = "Internal server error! Thrown from global exception middleware";
                    break;
            }

            _logger.LogError(exception.Message);
            await NegotiateContent(context, errorResponse);
        }

        private static async Task NegotiateContent (HttpContext context, ErrorResponse errorResponse) {
            var acceptHeader = context.Request.Headers["Accept"].ToString();

            if (acceptHeader.Contains("application/xml")) {
                context.Response.ContentType = "application/xml";
                using var streamWriter = new StringWriter();
                var xmlSerializer = new XmlSerializer(typeof(ErrorResponse));
                xmlSerializer.Serialize(streamWriter, errorResponse);
                await context.Response.WriteAsync(streamWriter.ToString());
            } else {
                context.Response.ContentType = "application/json";
                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}