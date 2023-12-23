using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace TaskHub.WebApi.Infrastructure
{
    public static class AuthenticationConfiguration
    {
        public static void ConfigureAuthentication(IServiceCollection services, IConfiguration config)
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
                    ValidateIssuerSigningKey = true,
                    NameClaimType = ClaimTypes.Name,
                    RoleClaimType = ClaimTypes.Role
                };
                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var nameClaim = context?.Principal?.FindFirst(ClaimTypes.Name);
                        if (nameClaim == null)
                        {
                            context?.Fail("NameClaimType is missing in the token.");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
