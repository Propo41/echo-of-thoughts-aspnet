using Cefalo.EchoOfThoughts.AppCore.Helpers;
using Cefalo.EchoOfThoughts.AppCore.Helpers.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Cefalo.EchoOfThoughts.WebApi.Extensions {
    public static class GlobalExceptionHandler {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger logger) {

            app.UseExceptionHandler(errorApp => {
                errorApp.Run(async context => {
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = error?.Error;

                    // Log the exception
                    logger.LogError(exception, "An unhandled exception occurred.");
                    ErrorResponse? errorResponse = null;

                    switch (exception) {
                        case NotFoundException ex:
                            errorResponse = new ErrorResponse {
                                StatusCode = (int) HttpStatusCode.NotFound,
                                Value = ex.Message,
                            };
                            break;

                        case BadRequestException ex:
                            errorResponse = new ErrorResponse {
                                StatusCode = (int) HttpStatusCode.BadRequest,
                                Value = ex.Message,
                            };
                            break;

                        case InvalidOperationException ex:
                            errorResponse = new ErrorResponse {
                                StatusCode = (int) HttpStatusCode.NotFound,
                                Value = ex.Message,
                            };
                            break;
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