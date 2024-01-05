using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskHub.Bll.Services;
using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Repositories;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.Helpers;
using TaskHub.WebApi.Middlewares;
using AutoMapper;
using TaskHub.Bll.Mappers;
using System.Security.Claims;

namespace TaskHub.WebApi.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static void Configure(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });
            services.Configure<JwtSettings>(config.GetSection("Jwt"));
            services.AddAutoMapper(conf =>
            {
                conf.AddProfiles(
                    new List<Profile>()
                    {
                        new UserMapperProfile(),
                        new TaskMapperProfile()
                    });
            });
            ApiControllerConfiguration.ConfigureApiBehaviorOptions(services);
            ConfigureFluentValidation(services);
            IdentityConfiguration.ConfigureIdentity(services, config);
            AuthenticationConfiguration.ConfigureAuthentication(services, config);

            return services;
        }
        public static void ConfigureFluentValidation(IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(typeof(Program));
        }
        public static IServiceCollection AddDependencyGroup(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
