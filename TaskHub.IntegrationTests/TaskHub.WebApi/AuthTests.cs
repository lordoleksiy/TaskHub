using IntegrationTests.TaskHub.WebApi;
using NSubstitute;
using System.Net.Http.Json;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.User;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using TaskHub.Common.DTO.Reponse.Token;

namespace TaskHub.IntegrationTests.TaskHub.WebApi
{
    [TestFixture]
    public class AuthControllerTests
    {
        private TestingWebAppFactory _factory;
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            _factory = new TestingWebAppFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task Register_WhenServiceReturnsSuccess_ReturnsSuccessResponse()
        {
            // Arrange
            var registerModel = new RegisterModel("testuser123", "testemail@gmail.com", "testpass12F_3");
            var apiResponse = new ApiResponse(Status.Success);
            _factory._authService.RegisterAsync(Arg.Any<RegisterModel>()).Returns(Task.FromResult(apiResponse));

            // Act
            var response = await _client.PostAsync("api/Auth/Register", JsonContent.Create(registerModel));

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var content = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            content.Status.Should().Be(apiResponse.Status);
            content.Message.Should().Be(apiResponse.Message);
            content.Errors.Should().BeEquivalentTo(apiResponse.Errors);
        }

        [Test]
        public async Task Register_WhenServiceReturnsNotSuccess_ReturnsBadRequestResponse()
        {
            // Arrange
            var registerModel = new RegisterModel("testuser123", "testemail@gmail.com", "testpass12F_3");
            var apiResponse = new ApiResponse(Status.Error);
            _factory._authService.RegisterAsync(Arg.Any<RegisterModel>()).Returns(Task.FromResult(apiResponse));

            // Act
            var response = await _client.PostAsync("api/Auth/Register", JsonContent.Create(registerModel));

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            content.Status.Should().Be(apiResponse.Status);
            content.Message.Should().Be(apiResponse.Message);
            content.Errors.Should().BeEquivalentTo(apiResponse.Errors);
        }

        [Test]
        public async Task Login_ServiceReturnsSuccessResponse_ReturnsSuccessResponseAndToken()
        {
            // Arrange
            var loginModel = new LoginModel("testuser123", "testpass12F_3");
            var apiResponse = new ApiResponse<TokenResponseDTO>(new TokenResponseDTO("token_string"));
            _factory._authService.LoginAsync(Arg.Any<LoginModel>()).Returns(Task.FromResult(apiResponse));

            // Act
            var response = await _client.PostAsync("api/Auth/Login", JsonContent.Create(loginModel));

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var content = JsonConvert.DeserializeObject<ApiResponse<TokenResponseDTO>>(await response.Content.ReadAsStringAsync());
            content.Status.Should().Be(apiResponse.Status);
            content.Message.Should().Be(apiResponse.Message);
            content.Errors.Should().BeEquivalentTo(apiResponse.Errors);
        }

        [Test]
        public async Task Login_ServiceReturnsNotSuccessResponse_ReturnsBadRequestResponse()
        {
            // Arrange
            var loginModel = new LoginModel("testuser123", "testpass12F_3");
            var apiResponse = new ApiResponse<TokenResponseDTO>(Status.Error);
            _factory._authService.LoginAsync(Arg.Any<LoginModel>()).Returns(Task.FromResult(apiResponse));

            // Act
            var response = await _client.PostAsync("api/Auth/Login", JsonContent.Create(loginModel));

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());
            content.Status.Should().Be(apiResponse.Status);
            content.Message.Should().Be(apiResponse.Message);
            content.Errors.Should().BeEquivalentTo(apiResponse.Errors);
        }
    }
}