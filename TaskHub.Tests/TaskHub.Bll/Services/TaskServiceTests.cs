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
        public async Task GetTasksByUserNameAsync_ReturnsApiResponse()
        {
            // Arrange
            var userName = "testuser";
            var filter = new TaskQueryParams();
            var users = new List<UserEntity> { new UserEntity { UserName = userName } };
            var tasks = new List<TaskEntity> { new TaskEntity() { Title = "testTask" } };
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUserByUserNameSpecification>()).Returns(users);
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFilteredTasksSpecification>()).Returns(tasks);

            // Act
            var result = await _taskService.GetTasksByUserNameAsync(userName, filter);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Success);
            result.Data.Should().NotBeNull();
        }

        [Test]
        public async Task CreateTaskAsync_ReturnsApiResponse()
        {
            // Arrange
            var newTaskDto = new NewTaskDTO();
            var taskEntity = new TaskEntity();
            var users = new List<UserEntity> { new UserEntity() };
            _mapper.Map<TaskEntity>(newTaskDto).Returns(taskEntity);
            _unitOfWork.UserRepository.GetAsync(Arg.Any<GetUsersByNamesSpecification>()).Returns(users);

            // Act
            var result = await _taskService.CreateTaskAsync(newTaskDto);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            // Add more assertions as needed
        }

        [Test]
        public async Task UpdateTaskAsync_ReturnsApiResponse()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDTO();
            var userName = "testuser";
            var taskEntity = new TaskEntity { AssignedUsers = new List<UserEntity> { new UserEntity { UserName = userName } } };
            var taskEntities = new List<TaskEntity> { taskEntity };
            _unitOfWork.TaskRepository.GetAsync(Arg.Any<GetFullTaskSpecification>()).Returns(taskEntities);

            // Act
            var result = await _taskService.UpdateTaskAsync(updateTaskDto, userName);

            // Assert
            result.Should().BeOfType<ApiResponse>();
            // Add more assertions as needed
        }

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
