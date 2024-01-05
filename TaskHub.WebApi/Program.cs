using TaskHub.WebApi.Infrastructure;
using Newtonsoft.Json;


namespace TaskHub.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.AddDependencyGroup(builder.Configuration);
           

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.Configure();

            app.MapControllers();

            app.Run();
        }
    }
}