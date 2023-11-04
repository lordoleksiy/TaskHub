using Microsoft.EntityFrameworkCore;
using TaskHub.Dal.Context;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Repositories;

namespace TaskHub.WebApi.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDependencyGroup(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DbContext, DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
