using Microsoft.AspNetCore.Mvc;
using TaskHub.Common.DTO.Reponse;

namespace TaskHub.WebApi.Infrastructure
{
    public static class ApiControllerConfiguration
    {
        public static void ConfigureApiBehaviorOptions(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var response = new ApiResponse("Validation Error", c.ModelState.Values.SelectMany(v => v.Errors).Select(a => a.ErrorMessage).ToList());
                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
