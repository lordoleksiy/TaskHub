using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TaskHub.Bll.Interfaces;
using TaskHub.Bll.Services;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
using TaskHub.Common.Enums;
using TaskHub.Common.QueryParams;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.CategorySpecifications;
using TaskHub.Dal.Specification.TaskSpecifications;
using TaskHub.Dal.Specification.UserSpecifications;
using TaskHub.Common.Constants;
using NSubstitute.ExceptionExtensions;

namespace TaskHub.Tests.Bll.Services
{
    [TestFixture]
    public class TaskServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private INotificationService _notificationService;
        private TaskService _taskService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _notificationService = Substitute.For<INotificationService>();
            _taskService = new TaskService(_unitOfWork, _mapper, _notificationService);
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
        public async Task CreateTaskAsync_WhenCalled_CallsUpdateCategoriesWithCorrectArguments()
        {
            // Arrange
            var newTask = new NewTaskDTO
            {
                Title = "Test Task",
                Categories = new List<string> { "category1", "category2" }
            };

            var taskEntity = new TaskEntity
            {
                Title = "Test Task"
            };

            _mapper.Map<TaskEntity>(Arg.Is(newTask)).Returns(taskEntity);
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>()).Returns(new List<UserEntity>());

            // Act
            await _taskService.CreateTaskAsync(newTask);

            // Assert
            await _taskService.Received(1).UpdateCategories(Arg.Is(newTask.Categories), Arg.Is(taskEntity));
        }


        //[Test]
        //public async Task CreateTaskAsync_WithInvalidParentTask_ReturnsApiResponseWithError()
        //{
        //    // Arrange
        //    var newTask = new NewTaskDTO
        //    {
        //        Title = "Test Task",
        //        // other properties
        //        ParentTaskId = "invalidParentTaskId" // Assuming an invalid parent task ID
        //    };

        //    _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>()).Returns(new List<UserEntity>());
        //    _unitOfWork.TaskRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((TaskEntity)null);

        //    // Act
        //    var result = await _taskService.CreateTaskAsync(newTask);

        //    // Assert
        //    result.Should().BeOfType<ApiResponse<TaskDTO>>();
        //    result.Status.Should().Be(Status.Error);
        //    result.Message.Should().Be(ResponseMessages.ErrorParentTask);
        //}

        //[Test]
        //public async Task CreateTaskAsync_WhenErrorOccurs_ReturnsApiResponseWithError()
        //{
        //    // Arrange
        //    var newTask = new NewTaskDTO
        //    {
        //        Title = "Test Task",
        //        // other properties
        //    };

        //    _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>()).Throws(new Exception("Some error occurred"));

        //    // Act
        //    var result = await _taskService.CreateTaskAsync(newTask);

        //    // Assert
        //    result.Should().BeOfType<ApiResponse<TaskDTO>>();
        //    result.Status.Should().Be(Status.Error);
        //    // Add more assertions based on the expected behavior of the method
        //}


        //[Test]
        //public async Task UpdateTaskAsync_ReturnsApiResponse()
        //{
        //    // Arrange
        //    var updateTaskDto = new UpdateTaskDTO();
        //    var userName = "testuser";
        //    var taskEntity = new TaskEntity { AssignedUsers = new List<UserEntity> { new UserEntity { UserName = userName } } };
        //    var taskEntities = new List<TaskEntity> { taskEntity };
        //    _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>()).Returns(taskEntities);
        //    //_mapper.Map<TaskEntity>(newTaskDto).Returns(taskEntity);

        //    // Act
        //    var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

        //    // Assert
        //    result.Should().BeOfType<ApiResponse>();
        //    // Add more assertions as needed
        //}

        [Test]
        public async Task DeleteTaskAsync_ReturnsApiResponse()
        {
            // Arrange
            var taskId = "someId";
            var taskEntity = new TaskEntity();
            _unitOfWork.TaskRepository.GetByIdAsync(Arg.Any<Guid>()).Returns(taskEntity);

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId, "testuser");

            // Assert
            result.Should().BeOfType<ApiResponse>();
            // Add more assertions as needed
        }
    }
}
