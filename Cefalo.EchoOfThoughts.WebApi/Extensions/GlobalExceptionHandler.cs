using Cefalo.EchoOfThoughts.Domain;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Xml.Serialization;

namespace Cefalo.EchoOfThoughts.WebApi.Extensions {
    public static class GlobalExceptionHandler {
        public static void UseCustomExceptionHandler (this IApplicationBuilder app, ILogger logger) {

            app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = error?.Error;

                    // Log the exception
                    logger.LogError(exception, "An unhandled exception occurred.");
                    ErrorResponse? errorResponse = null;

                    switch (exception) {
                        case UnauthorizedAccessException ex:
                            errorResponse = new ErrorResponse {
                                StatusCode = (int) HttpStatusCode.Forbidden,
                                Value = ex.Message,
                            };
                            break;

                        default:
                            errorResponse = new ErrorResponse {
                                StatusCode = (int) HttpStatusCode.InternalServerError,
                                Value = exception?.Message,
                            };
                            break;
                    }
                    context.Response.StatusCode = errorResponse.StatusCode;

                    var jsonResponse = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(jsonResponse);
                });

            });
        }
    }
}