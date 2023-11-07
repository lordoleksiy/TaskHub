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

namespace TaskHub.WebApi.Infrastructure
{
    public static class ServicesConfiguration
    {
        public static void Configure(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.Configure<JwtSettings>(config.GetSection("Jwt"));
            ConfigureApiBehaviorOptions(services);
            ConfigureFluentValidation(services);
            ConfigureIdentity(services, config);
            ConfigureAuthentication(services, config);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        public static IServiceCollection AddDependencyGroup(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthSerivce, AuthService>();

            return services;
        }

        private static void ConfigureApiBehaviorOptions(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var response = new ApiResponse()
                    {
                        Message = "Validation Error",
                        Status = Status.Error,
                        Errors = c.ModelState.Values.SelectMany(v => v.Errors).Select(a => a.ErrorMessage).ToList()
                    };
                    return new BadRequestObjectResult(response);
                };
            });
        }

        private static void ConfigureFluentValidation(IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining(typeof(Program));
        }

        private static void ConfigureIdentity(IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<DataContext>();
        }

        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}
