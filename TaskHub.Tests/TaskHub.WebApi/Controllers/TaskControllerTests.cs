using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Common.QueryParams;
using TaskHub.WebApi.Controllers;

namespace TaskHub.Tests.TaskHub.WebApi.Controllers
{

    [TestFixture]
    public class TaskControllerTests
    {
        private TaskController _taskController;
        private ITaskService _taskService;

        [SetUp]
        public void Setup()
        {
            _taskService = Substitute.For<ITaskService>();
            _taskController = new TaskController(_taskService);
        }

        private ControllerContext CreateMockControllerContext(string expectedUserName)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, expectedUserName)
            }, "mock"));
            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            return new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Test]
        public async Task GetMyTasksAsync_WhenAuthorized_ReturnsOkResultWithTasks()
        {
            // Arrange
            var filter = new TaskQueryParams();
            var expectedUserName = "testuser";
            var expectedTasks = new List<TaskDTO>();


            _taskService.GetTasksByUserNameAsync(Arg.Is(expectedUserName), Arg.Is(filter)).Returns(new ApiResponse<IEnumerable<TaskDTO>>(expectedTasks));

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.GetMyTasksAsync(filter);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType<ApiResponse<IEnumerable<TaskDTO>>>();
            var response = okObjectResult.Value as ApiResponse<IEnumerable<TaskDTO>>;
            response.Data.Should().BeEquivalentTo(expectedTasks);
        }

        [Test]
        public async Task CreateTaskAsync_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var task = new NewTaskDTO();
            var expectedUserName = "testuser";
            var expectedResponse = new ApiResponse<TaskDTO>(new TaskDTO());

            _taskService.CreateTaskAsync(Arg.Is(task)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.CreateTaskAsync(task);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().Be(expectedResponse);
        }

        [Test]
        public async Task UpdateTaskAsync_ValidModel_ReturnsOkResult()
        {
            // Arrange
            var task = new UpdateTaskDTO();
            var expectedUserName = "testuser";
            var expectedResponse = new ApiResponse<TaskDTO>(new TaskDTO());

            _taskService.UpdateTaskAsync(Arg.Is(task), Arg.Is(expectedUserName)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.UpdateTaskAsync(task);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().Be(expectedResponse);
        }

        [Test]
        public async Task DeleteTaskAsync_ValidId_ReturnsOkResult()
        {
            // Arrange
            var taskId = "testId";
            var expectedUserName = "testuser";
            var expectedResponse = new ApiResponse(Status.Success);

            _taskService.DeleteTaskAsync(Arg.Is(taskId), Arg.Is(expectedUserName)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.DeleteTaskAsync(taskId);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().Be(expectedResponse);
        }

        [Test]
        public async Task GetMyTasksAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var filter = new TaskQueryParams();
            var expectedUserName = "testuser";
            var expectedErrorMessage = "Error message";
            var expectedResponse = new ApiResponse<IEnumerable<TaskDTO>>(Status.Error, expectedErrorMessage);

            _taskService.GetTasksByUserNameAsync(Arg.Is(expectedUserName), Arg.Is(filter)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.GetMyTasksAsync(filter);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(expectedResponse);
        }

        [Test]
        public async Task CreateTaskAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var task = new NewTaskDTO();
            var expectedUserName = "testuser";
            var expectedErrorMessage = "Error message";
            var expectedResponse = new ApiResponse<TaskDTO>(Status.Error, expectedErrorMessage);

            _taskService.CreateTaskAsync(Arg.Is(task)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.CreateTaskAsync(task);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(expectedResponse);
        }

        [Test]
        public async Task UpdateTaskAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var task = new UpdateTaskDTO();
            var expectedUserName = "testuser";
            var expectedErrorMessage = "Error message";
            var expectedResponse = new ApiResponse<TaskDTO>(Status.Error, expectedErrorMessage);

            _taskService.UpdateTaskAsync(Arg.Is(task), Arg.Is(expectedUserName)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.UpdateTaskAsync(task);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(expectedResponse);
        }

        [Test]
        public async Task DeleteTaskAsync_WhenServiceReturnsError_ReturnsBadRequest()
        {
            // Arrange
            var taskId = "testId";
            var expectedUserName = "testuser";
            var expectedErrorMessage = "Error message";
            var expectedResponse = new ApiResponse(Status.Error, expectedErrorMessage);

            _taskService.DeleteTaskAsync(Arg.Is(taskId), Arg.Is(expectedUserName)).Returns(expectedResponse);

            _taskController.ControllerContext = CreateMockControllerContext(expectedUserName);

            // Act
            var result = await _taskController.DeleteTaskAsync(taskId);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be(expectedResponse);
        }
    }
}
