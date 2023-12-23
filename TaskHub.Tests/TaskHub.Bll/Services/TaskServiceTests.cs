using AutoMapper;
using NSubstitute;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Common.Enums;
using TaskHub.Common.QueryParams;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.TaskSpecifications;
using TaskHub.Dal.Specification.UserSpecifications;
using TaskHub.Common.Constants;

namespace TaskHub.Tests.Bll.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private INotificationService _notificationService;
        private ICategoryService _categoryService;
        private TaskService _taskService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _notificationService = Substitute.For<INotificationService>();
            _categoryService = Substitute.For<ICategoryService>();
            _taskService = new TaskService(_unitOfWork, _mapper, _notificationService, _categoryService);
        }

        [Test]
        public async Task GetTasksByUserNameAsync_ReturnsApiResponseWithTasks()
        {
            // Arrange
            var userName = "testuser";
            var filter = new TaskQueryParams();
            var users = new List<UserEntity> { new UserEntity { UserName = userName } };
            var tasksDTO = new List<TaskDTO> { new TaskDTO() { Title = "testTask" } };
            var tasks = new List<TaskEntity> { new TaskEntity() { Title = "testTask" } };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUserByUserNameSpecification>()).Returns(users);
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFilteredTasksSpecification>()).Returns(tasks);
            _mapper.Map<IEnumerable<TaskDTO>>(Arg.Is(tasks)).Returns(tasksDTO);

            // Act
            var result = await _taskService.GetTasksByUserNameAsync(userName, filter);

            // Assert
            result.Should().BeOfType<ApiResponse<IEnumerable<TaskDTO>>>();
            result.Status.Should().Be(Status.Success);
            result.Data.Should().NotBeNull();
            result.Data.Should().BeSameAs(tasksDTO);
        }

        [Test]
        public async Task GetTasksByUserNameAsync_ReturnsApiResponseWithError()
        {
            // Arrange
            var userName = "testuser";
            var filter = new TaskQueryParams();
            var tasksDTO = new List<TaskDTO> { new TaskDTO() { Title = "testTask" } };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUserByUserNameSpecification>()).Returns(new List<UserEntity>());

            // Act
            var result = await _taskService.GetTasksByUserNameAsync(userName, filter);

            // Assert
            result.Should().BeOfType<ApiResponse<IEnumerable<TaskDTO>>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.UserNotFound);
        }

        [Test]
        public async Task CreateTaskAsync_WithParentTask_ReturnsApiResponseWithTask()
        {
            // Arrange
            var newTask = new NewTaskDTO
            {
                Title = "Test Task",
                ParentTaskId = Guid.NewGuid().ToString()
            };

            var taskEntity = new TaskEntity
            {
                Title = "Test Task"
            };

            var taskDTO = new TaskDTO
            {
                Title = "Test Task"
            };
            var parentTask = new TaskEntity { Id = Guid.Parse(newTask.ParentTaskId) };

            _mapper.Map<TaskEntity>(Arg.Is(newTask)).Returns(taskEntity);
            _mapper.Map<TaskDTO>(Arg.Is(taskEntity)).Returns(taskDTO);
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>()).Returns(new List<UserEntity>());
            _unitOfWork.TaskRepository.GetByIdAsync(Arg.Is<Guid>(id => id == parentTask.Id)).Returns(parentTask);

            // Act
            var result = await _taskService.CreateTaskAsync(newTask);

            // Assert
            await _unitOfWork.Received(1).Commit();
            await _unitOfWork.TaskRepository.Received(1).AddAsync(Arg.Is(taskEntity));
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Success);
            result.Data.Should().Be(taskDTO);
        }

        [Test]
        public async Task CreateTaskAsync_WithInvalidParentTaskId_ReturnsApiResponseWithError()
        {
            // Arrange
            var newTask = new NewTaskDTO
            {
                Title = "Test Task",
                ParentTaskId = Guid.NewGuid().ToString()
            };

            var taskEntity = new TaskEntity
            {
                Title = "Test Task"
            };

            _mapper.Map<TaskEntity>(Arg.Is(newTask)).Returns(taskEntity);
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>()).Returns(new List<UserEntity>());
            _unitOfWork.TaskRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((TaskEntity)null);

            // Act
            var result = await _taskService.CreateTaskAsync(newTask);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.ErrorParentTask);
        }

        [Test]
        public async Task DeleteTaskAsync_ReturnsSuccessApiResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid().ToString();
            var taskEntity = new TaskEntity();
            _unitOfWork.TaskRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(taskEntity);

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId, "testuser");

            // Assert
            await _unitOfWork.Received(1).Commit();
            _unitOfWork.TaskRepository.Received(1).Remove(Arg.Is(taskEntity));
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Success);
            result.Message.Should().Be(ResponseMessages.TaskDeletedSuccessfully);
        }

        [Test]
        public async Task DeleteTaskAsync_ReturnsErrorApiResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid().ToString();
            _unitOfWork.TaskRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((TaskEntity)null);

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId, "testuser");

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.NoTaskFound);
        }

        [Test]
        public async Task UpdateTaskAsync_WhenTaskNotFound_ReturnsErrorApiResponse()
        {
            // Arrange
            var nonExistingTaskId = Guid.NewGuid().ToString();
            var updateTaskDto = new UpdateTaskDTO { Id = nonExistingTaskId };
            var userName = "testuser";
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>())
                .Returns(new List<TaskEntity>());

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.NoTaskFound);
        }

        [Test]
        public async Task UpdateTaskAsync_WhenNoAssignedUsers_ReturnsErrorApiResponse()
        {
            // Arrange
            var existingTaskId = Guid.NewGuid().ToString();
            var updateTaskDto = new UpdateTaskDTO { Id = existingTaskId, AssignedUserNames = null };
            var userName = "testuser";
            var taskEntity = new TaskEntity { AssignedUsers = new List<UserEntity>() };
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>())
                .Returns(new List<TaskEntity> { taskEntity });

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.TaskCannotBeWithoutUsers);
        }

        [Test]
        public async Task UpdateTaskAsync_WithValidDataAndAssignedUsers_ReturnsSuccessApiResponse()
        {
            // Arrange
            var existingTaskId = Guid.NewGuid().ToString();
            var updateTaskDto = new UpdateTaskDTO
            {
                Id = existingTaskId,
                Title = "Updated Task",
                AssignedUserNames = new List<string> { "user1", "user2" },
                Status = TaskStatusCode.Closed,
                DueDate = DateTime.Now.ToString()
            };
            var userName = "user1";
            var taskEntity = new TaskEntity
            {
                Id = Guid.Parse(existingTaskId),
                Title = "Original Task",
                AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "user1" } }
            };
            var taskDTO = new TaskDTO()
            {
                Title = "Final Task"
            };
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>())
                .Returns(new List<TaskEntity> { taskEntity });
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>())
                .Returns(new List<UserEntity> { new UserEntity { UserName = "user1" }, new UserEntity { UserName = "user2" } });
            _categoryService.UpdateCategoriesAsync(Arg.Any<List<string>>())
                .Returns(new List<CategoryEntity>());
            _mapper.Map<TaskDTO>(Arg.Is(taskEntity)).Returns(taskDTO);

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Success);
            result.Data.Should().Be(taskDTO);
        }

        [Test]
        public async Task UpdateTaskAsync_WhenTaskCannotBeUpdated_ReturnsErrorApiResponse()
        {
            // Arrange
            var existingTaskId = Guid.NewGuid().ToString();
            var updateTaskDto = new UpdateTaskDTO
            {
                Id = existingTaskId,
                Title = "Updated Task",
                AssignedUserNames = new List<string> { "user1", "user2" },
                Status = TaskStatusCode.Closed,
                DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            var userName = "user3"; // Assuming user3 is not among the assigned users
            var taskEntity = new TaskEntity
            {
                Id = Guid.Parse(existingTaskId),
                Title = "Original Task",
                AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "user1" }, new UserEntity { UserName = "user2" } }
            };
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>())
                .Returns(new List<TaskEntity> { taskEntity });
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>())
                .Returns(new List<UserEntity> { new UserEntity { UserName = "user1" }, new UserEntity { UserName = "user2" } });

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.TaskCannotBeUpdated);
        }

        [Test]
        public async Task UpdateTaskAsync_WhenNoUsersFound_ReturnsErrorApiResponse()
        {
            // Arrange
            var existingTaskId = Guid.NewGuid().ToString();
            var updateTaskDto = new UpdateTaskDTO
            {
                Id = existingTaskId,
                Title = "Updated Task",
                AssignedUserNames = new List<string> { "user1", "user2" },
                Status = TaskStatusCode.Closed,
                DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            var userName = "user1"; // Assuming user1 is among the assigned users
            var taskEntity = new TaskEntity
            {
                Id = Guid.Parse(existingTaskId),
                Title = "Original Task",
                AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "user1" }, new UserEntity { UserName = "user2" } }
            };
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>())
                .Returns(new List<TaskEntity> { taskEntity });
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>())
                .Returns(new List<UserEntity>()); // Assuming no users found

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.NoUsersFound);
        }

        [Test]
        public async Task UpdateTaskAsync_WhenSubTasksNotClosed_ReturnsErrorApiResponse()
        {
            // Arrange
            var existingTaskId = Guid.NewGuid().ToString();
            var updateTaskDto = new UpdateTaskDTO
            {
                Id = existingTaskId,
                Title = "Updated Task",
                AssignedUserNames = new List<string> { "user1", "user2" },
                Status = TaskStatusCode.Closed,
                DueDate = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ")
            };
            var userName = "user1";
            var subTask1 = new TaskEntity { Status = TaskStatusCode.InProgress };
            var subTask2 = new TaskEntity { Status = TaskStatusCode.Closed };
            var taskEntity = new TaskEntity
            {
                Id = Guid.Parse(existingTaskId),
                Title = "Original Task",
                AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "user1" }, new UserEntity { UserName = "user2" } },
                Subtasks = new List<TaskEntity> { subTask1, subTask2 }
            };
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>())
                .Returns(new List<TaskEntity> { taskEntity });
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>())
                .Returns(new List<UserEntity> { new UserEntity { UserName = "user1" }, new UserEntity { UserName = "user2" } });
            _categoryService.UpdateCategoriesAsync(Arg.Any<List<string>>())
                .Returns(new List<CategoryEntity>());

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse<TaskDTO>>();
            result.Status.Should().Be(Status.Error);
            result.Message.Should().Be(ResponseMessages.SubTasksMustBeClosed);
        }
    }
}
