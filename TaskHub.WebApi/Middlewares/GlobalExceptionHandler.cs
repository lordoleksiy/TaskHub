using System.Text.Json;
using TaskHub.Common.DTO.Reponse;

namespace TaskHub.WebApi.Middlewares
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;

                var result = new ApiResponse(Status.Error, error.Message);
                await response.WriteAsync(JsonSerializer.Serialize(result));
            }
        }
    }
}
