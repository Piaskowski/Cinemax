
using Cinemax.Application.Exceptions;
using Cinemax.Shared.Contracts.Common;

namespace Cinemax.Server.Middlewares
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }catch (ValidationException e)
            {
                _logger.LogWarning(e, e.Message);

                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "Wystąpiły błędy",
                    Errors = [.. e.Errors]
                });
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning(e, e.Message);

                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = e.Message,
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "Coś poszło nie tak. Skontaktuj się z administratorem!"
                });
            }
        }
    }
}
