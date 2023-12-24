using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services;
using TaskHub.Common.Helpers;
using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Repositories;
using TaskHub.WebApi.Infrastructure;
using TaskHub.WebApi.Middlewares;

namespace TaskHub.Tests.TaskHub.WebApi.Infrastructure
{
    public class ServicesConfigurationTests
    {
        [Test]
        public void AddDependencyGroup_Should_Register_All_Services()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = Substitute.For<IConfiguration>();

            configuration.GetConnectionString("DefaultConnection").Returns("connection string");

            // Act
            services.AddDependencyGroup(configuration);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(services.Any(descriptor => descriptor.ServiceType == typeof(DataContext)), Is.True);
                Assert.That(services.Any(descriptor => descriptor.ServiceType == typeof(IUnitOfWork)), Is.True);
                Assert.That(services.Any(descriptor => descriptor.ServiceType == typeof(IAuthService)), Is.True);
                Assert.That(services.Any(descriptor => descriptor.ServiceType == typeof(ITaskService)), Is.True);
                Assert.That(services.Any(descriptor => descriptor.ServiceType == typeof(INotificationService)), Is.True);
                Assert.That(services.Any(descriptor => descriptor.ServiceType == typeof(ICategoryService)), Is.True);
            });
        }

        [Test]
        public void ConfigureServices_ShouldConfigureJWTSettings()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "your_jwt_key"},
                    {"Jwt:Issuer", "your_issuer"},
                    {"Jwt:Audience", "your_audience"}
                })
                .Build();

            // Act
            services.ConfigureServices(configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var jwtSettings = serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
            jwtSettings.Should().NotBeNull();
            jwtSettings.Key.Should().Be("your_jwt_key");
            jwtSettings.Issuer.Should().Be("your_issuer");
            jwtSettings.Audience.Should().Be("your_audience");
        }

        [Test]
        public void ConfigureServices_ShouldConfigureServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "your_jwt_key" },
                })
                .Build();
            var dataContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeDataContext")
                .Options;
            services.AddDbContext<DataContext>(opt => new DataContext(dataContextOptions));
            services.AddLogging();

            // Act
            ServicesConfiguration.ConfigureServices(services, configuration);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetRequiredService<IMapper>().Should().NotBeNull();
            serviceProvider.GetRequiredService<IOptions<ApiBehaviorOptions>>().Value.Should().NotBeNull();
            serviceProvider.GetRequiredService<IOptions<JsonOptions>>().Value.Should().NotBeNull();
            serviceProvider.GetRequiredService<UserManager<UserEntity>>().Should().NotBeNull();
            serviceProvider.GetRequiredService<RoleManager<RoleEntity>>().Should().NotBeNull();
        }

        [Test]
        public void ConfigureAuthentication_WhenCalled_ShouldConfigureJwtBearerAuthentication()
        {
            // Arrange
            var services = new ServiceCollection();
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
            {"Jwt:Issuer", "issuer"},
            {"Jwt:Audience", "audience"},
            {"Jwt:Key", "key"}
                })
                .Build();

            // Act
            services.ConfigureServices(config);

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var authenticationScheme = serviceProvider.GetRequiredService<IOptions<AuthenticationOptions>>().Value.DefaultAuthenticateScheme;
            authenticationScheme.Should().Be(JwtBearerDefaults.AuthenticationScheme);
        }
    }
}
