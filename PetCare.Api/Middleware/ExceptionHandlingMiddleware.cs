using PetCare.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace PetCare.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ErrorDetails
            {
                Success = false,
                Message = exception.Message
            };

            switch (exception)
            {
                case BadRequestException badRequestEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = "Wystąpiły błędy walidacji.";
                    errorResponse.Errors = badRequestEx.ValidationErrors;
                    break;

                case NotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = notFoundEx.Message;
                    break;

                default:
                    _logger.LogError(exception, "Nieoczekiwany błąd serwera.");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Wystąpił wewnętrzny błąd serwera. Skontaktuj się z administratorem.";
                    break;
            }

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }

    public class ErrorDetails
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}

