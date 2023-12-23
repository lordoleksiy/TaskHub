using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;

namespace TaskHub.WebApi.Infrastructure
{
    public static class IdentityConfiguration
    {
        public static void ConfigureIdentity(IServiceCollection services, IConfiguration config)
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
    }
}
