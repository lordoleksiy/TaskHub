using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TaskHub.Bll.Services;
using TaskHub.Common.DTO.Notification;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Common.DTO.Task;
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
        public async Task UpdateForTask_ShouldAddForEachAssignedUser()
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
            
            _unitOfWork.NotificationRepository.GetAsync(Arg.Any<GetNotificationsByUserName>())
                .Returns(new List<NotificationEntity> { existingNotification });

            // Act
            await _notificationService.UpdateForTask(task);

            // Assert
            _unitOfWork.NotificationRepository.Received(2).Update(Arg.Any<NotificationEntity>());
            _unitOfWork.Received(1);
        }

        [Test]
        public async Task UpdateForTask_ShouldUpdateNotificationsForEachAssignedUser()
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

            _unitOfWork.NotificationRepository.GetAsync(Arg.Any<GetNotificationsByUserName>())
                .Returns(new List<NotificationEntity> { existingNotification });

            // Act
            await _notificationService.UpdateForTask(task);

            // Assert
            _unitOfWork.NotificationRepository.Received(2).Update(Arg.Any<NotificationEntity>());
            _unitOfWork.Received(1);
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
            result.Should().BeOfType<ApiResponse>();
            result.Status.Should().Be(Status.Success);
            var notifications = result.Data as IEnumerable<NotificationDTO>;
            notifications.Should().NotBeNull();
            notifications.Should().HaveCount(2);
        }
    }
}
