using AutoMapper;
using TaskHub.Bll.Mappers;
using TaskHub.Common.DTO.Task;
using TaskHub.Dal.Entities;

namespace TaskHub.Tests.TaskHub.Bll.Mappers
{
    [TestFixture]
    public class TaskMapperProfileTests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TaskMapperProfile());
            });
            _mapper = configuration.CreateMapper();
        }

        [Test]
        public void Map_NewTaskDtoToTaskEntity_ShouldMapCorrectly()
        {
            // Arrange
            var newTaskDto = new NewTaskDTO { DueDate = "2023-12-31" };

            // Act
            var taskEntity = _mapper.Map<TaskEntity>(newTaskDto);

            // Assert
            taskEntity.Should().NotBeNull();
            taskEntity.DueDate.Should().Be(DateTime.Parse(newTaskDto.DueDate));
            taskEntity.Categories.Should().BeNull();
            taskEntity.AssignedUsers.Should().BeNull();
        }

        [Test]
        public void Map_UpdateTaskDtoToTaskEntity_ShouldMapCorrectly()
        {
            // Arrange
            var updateTaskDto = new UpdateTaskDTO { DueDate = "2023-12-31" };

            // Act
            var taskEntity = _mapper.Map<TaskEntity>(updateTaskDto);

            // Assert
            taskEntity.Should().NotBeNull();
            taskEntity.DueDate.Should().Be(DateTime.Parse(updateTaskDto.DueDate));
            taskEntity.Id.Should().Be(Guid.Empty);
            taskEntity.Categories.Should().BeNull();
            taskEntity.AssignedUsers.Should().BeNull();
        }

        [Test]
        public void Map_TaskEntityToTaskDto_ShouldMapCorrectly()
        {
            // Arrange
            var taskEntity = new TaskEntity
            {
                Id = Guid.NewGuid(),
                DueDate = DateTime.UtcNow,
                ParentTaskId = Guid.NewGuid(),
                AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "User1" } },
                Categories = new List<CategoryEntity> { new CategoryEntity { Name = "Category1" } }
            };

            // Act
            var taskDto = _mapper.Map<TaskDTO>(taskEntity);

            // Assert
            taskDto.Should().NotBeNull();
            taskDto.Id.Should().Be(taskEntity.Id.ToString());
            taskDto.ParentTaskId.Should().Be(taskEntity.ParentTaskId.ToString());
            taskDto.AssignedUserNames.Should().ContainSingle().Which.Should().Be("User1");
            taskDto.Categories.Should().ContainSingle().Which.Should().Be("Category1");
        }
    }
}
