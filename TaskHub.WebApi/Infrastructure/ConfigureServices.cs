using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Context;

namespace TaskHub.WebApi.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDependencyGroup(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
