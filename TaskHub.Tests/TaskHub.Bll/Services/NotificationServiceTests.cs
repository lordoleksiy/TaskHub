using AutoMapper;
using NSubstitute;
using TaskHub.Bll.Services;
using TaskHub.Common.DTO.Notification;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Specification.NotificationSpecifications;

namespace TaskHub.Tests.TaskHub.Bll.Services
{
    [TestFixture]
    public class NotificationServiceTests
    {
        private NotificationService _notificationService;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _notificationService = new NotificationService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task CreateForTask_ShouldAddNotificationsForEachAssignedUser()
        {
            // Arrange
            var task = new TaskEntity
            {
                AssignedUsers = new List<UserEntity>
                {
                    new UserEntity { UserName = "user1" },
                    new UserEntity { UserName = "user2" }
                }
            };

            // Act
            await _notificationService.CreateForTask(task);

            // Assert
            await _unitOfWork.NotificationRepository.Received(2).AddAsync(Arg.Any<NotificationEntity>());
            _unitOfWork.Received(1);
        }

        [Test]
        public async Task UpdateForTask_UpdatesExistingNotifications()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new() { UserName = "testuser" },
                new() { UserName = "testuser2" }
            };
            var task = new TaskEntity
            {
                DueDate = DateTime.Now,
                AssignedUsers = users
            };

            var existingNotifications = users.Select(user => new NotificationEntity
            {
                User = user,
                Task = task,
                DueDate = DateTime.Now.AddDays(-1)
            }).ToList();

            _unitOfWork.NotificationRepository.GetAsync(Arg.Any<GetNotificationsByUserName>())
                .Returns(existingNotifications);

            // Act
            await _notificationService.UpdateForTask(task);

            // Assert
            _unitOfWork.NotificationRepository.Received(users.Count).Update(Arg.Is<NotificationEntity>(n => n.Task == task && n.DueDate == task.DueDate));
            await _unitOfWork.Received(1).Commit();
        }


        [Test]
        public async Task UpdateForTask_CreatesNewNotifications()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new() { UserName = "testuser" },
                new() { UserName = "testuser2" }
            };
            var task = new TaskEntity 
            { 
                DueDate = DateTime.Now,
                AssignedUsers = users
            };

            _unitOfWork.NotificationRepository.GetAsync(Arg.Any<GetNotificationsByUserName>())
                .Returns(new List<NotificationEntity>());

            // Act
            await _notificationService.UpdateForTask(task);

            // Assert
            await _unitOfWork.NotificationRepository.Received(users.Count).AddAsync(Arg.Is<NotificationEntity>(n => n.Task == task));
            await _unitOfWork.Received(1).Commit();
        }


        [Test]
        public async Task Get_ShouldReturnApiResponseWithNotificationsForUser()
        {
            // Arrange
            var userName = "testUser";
            var notificationsEntities = new List<NotificationEntity>
            {
                new NotificationEntity
                {
                    User = new UserEntity { UserName = userName },
                    Task = new TaskEntity { Title = "Task1" },
                    DueDate = DateTime.UtcNow
                },
                new NotificationEntity
                {
                    User = new UserEntity { UserName = userName },
                    Task = new TaskEntity { Title = "Task2" },
                    DueDate = DateTime.UtcNow.AddDays(1)
                }
            };
            _unitOfWork.NotificationRepository.GetAsync(Arg.Any<GetNotificationsByUserName>())
                .Returns(notificationsEntities);

            // Act
            var result = await _notificationService.Get(userName);

            // Assert
            result.Should().BeOfType<ApiResponse<IEnumerable<NotificationDTO>>>();
            result.Status.Should().Be(Status.Success);
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
        }
    }
}
