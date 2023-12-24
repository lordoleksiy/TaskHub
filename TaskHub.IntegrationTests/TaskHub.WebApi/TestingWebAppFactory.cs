using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using TaskHub.Bll.Interfaces;
using TaskHub.Dal.Context;
using TaskHub.WebApi;

namespace IntegrationTests.TaskHub.WebApi
{
    public class TestingWebAppFactory : WebApplicationFactory<Program>
    {
        public readonly IAuthService _authService;
        public readonly ICategoryService _categoryService;
        public readonly INotificationService _notificationService;
        public readonly ITaskService _taskService;

        public TestingWebAppFactory()
        {
            _authService = Substitute.For<IAuthService>();
            _categoryService = Substitute.For<ICategoryService>();
            _notificationService = Substitute.For<INotificationService>();
            _taskService = Substitute.For<ITaskService>();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<DataContext>));
                services.AddSqlServer<DataContext>(GetConnectionString());

                services.AddSingleton(_authService);
                services.AddSingleton(_categoryService);
                services.AddSingleton(_notificationService);
                services.AddSingleton(_taskService);

                var serviceProvider = services.BuildServiceProvider();
                var scope = serviceProvider.CreateScope();
            });
        }

        private string GetConnectionString()
        {
            return "Server=(localdb)\\mssqllocaldb;Database=TestTaskHubDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";
        }
    }
}
