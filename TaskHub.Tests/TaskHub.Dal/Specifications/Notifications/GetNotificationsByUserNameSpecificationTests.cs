using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.NotificationSpecifications;

namespace TaskHub.Tests.TaskHub.Dal.Specifications.Notifications
{
    [TestFixture]
    public class GetNotificationsByUserNameSpecificationTests
    {
        [Test]
        public void GetNotificationsByUserName_ShouldFilterNotificationsByUserName()
        {
            // Arrange
            var notifications = new List<NotificationEntity>
            {
                new NotificationEntity { Id = Guid.NewGuid(), User = new UserEntity { UserName = "User1" }, Task = new TaskEntity { Id = Guid.NewGuid(), Title = "Task 1" } },
                new NotificationEntity { Id = Guid.NewGuid(), User = new UserEntity { UserName = "User2" }, Task = new TaskEntity { Id = Guid.NewGuid(), Title = "Task 2" } },
                new NotificationEntity { Id = Guid.NewGuid(), User = new UserEntity { UserName = "User1" }, Task = new TaskEntity { Id = Guid.NewGuid(), Title = "Task 3" } }
            };
            var userName = "User1";
            var spec = new GetNotificationsByUserName(userName);

            // Act
            var results = spec.Evaluate(notifications.AsQueryable()).ToList();

            // Assert
            results.Should().HaveCount(2);
            results.Should().Contain(r => r.User.UserName == userName);
        }
    }
}
