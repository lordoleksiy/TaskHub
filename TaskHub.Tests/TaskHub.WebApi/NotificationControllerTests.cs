using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.DTO.Notification;
using TaskHub.Common.DTO.Reponse;
using TaskHub.WebApi.Controllers;

namespace TaskHub.Tests.TaskHub.WebApi
{
    [TestFixture]
    public class NotificationControllerTests
    {
        private NotificationController _notificationController;
        private INotificationService _notificationService;

        [SetUp]
        public void Setup()
        {
            _notificationService = Substitute.For<INotificationService>();
            _notificationController = new NotificationController(_notificationService);
        }

        [Test]
        public async Task Get_WhenAuthorized_ReturnsOkResultWithNotifications()
        {
            // Arrange
            var expectedUserName = "testuser";
            var expectedNotifications = new List<NotificationDTO>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, expectedUserName)
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            _notificationService.Get(Arg.Is(expectedUserName)).Returns(new ApiResponse<IEnumerable<NotificationDTO>>(expectedNotifications));

            _notificationController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await _notificationController.Get();

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType<ApiResponse<IEnumerable<NotificationDTO>>>();
            var response = okObjectResult.Value as ApiResponse<IEnumerable<NotificationDTO>>;
            response.Data.Should().BeEquivalentTo(expectedNotifications);
        }
    }
}
