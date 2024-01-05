using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.DTO.Reponse.Token;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.User;
using TaskHub.WebApi.Controllers;

namespace TaskHub.Tests.TaskHub.WebApi.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private IAuthService _authService;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _authService = Substitute.For<IAuthService>();
            _authController = new AuthController(_authService);
        }

        [Test]
        public async Task Register_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var model = new RegisterModel("username", "email@example.com", "password");
            var responseModel = new ApiResponse(Status.Success);

            _authService.RegisterAsync(Arg.Is(model)).Returns(responseModel);

            // Act
            var actionResult = await _authController.Register(model);
            var okObjectResult = actionResult.Result as OkObjectResult;
            var response = okObjectResult!.Value as ApiResponse;

            // Assert
            okObjectResult.Should().NotBeNull();
            response.Should().NotBeNull();
            response!.Status.Should().Be(Status.Success);
        }

        [Test]
        public async Task Register_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new RegisterModel("username", "email@example.com", "password");
            var response = new ApiResponse(Status.Error, "Invalid model");

            _authService.RegisterAsync(Arg.Is(model)).Returns(response);

            // Act
            var result = await _authController.Register(model);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be(response);
        }

        [Test]
        public async Task Login_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var model = new LoginModel("username", "password");
            var response = new ApiResponse<TokenResponseDTO>(new TokenResponseDTO("token"));

            _authService.LoginAsync(Arg.Is(model)).Returns(response);

            // Act
            var result = await _authController.Login(model);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(response);
        }

        [Test]
        public async Task Login_InvalidModel_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new LoginModel("username", "password");
            var response = new ApiResponse<TokenResponseDTO>(Status.Error, "Invalid model");

            _authService.LoginAsync(Arg.Is(model)).Returns(response);

            // Act
            var result = await _authController.Login(model);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be(response);
        }

    }
}
