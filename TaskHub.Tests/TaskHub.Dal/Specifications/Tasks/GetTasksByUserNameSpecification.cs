using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.TaskSpecifications;

namespace TaskHub.Dal.Tests.Specifications
{
    [TestFixture]
    public class GetTasksByUserNameSpecificationTests
    {
        [Test]
        public void ShouldFilterTasksByUserNameAndIncludeAssignedUsersAndCategories()
        {
            // Arrange
            var userName = "User1";
            var tasks = new List<TaskEntity>
            {
                new TaskEntity
                {
                    AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "User1" } },
                    Categories = new List<CategoryEntity> { new CategoryEntity() }
                },
                new TaskEntity
                {
                    AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "User2" } },
                    Categories = new List<CategoryEntity> { new CategoryEntity() }
                },
                new TaskEntity
                {
                    AssignedUsers = new List<UserEntity> { new UserEntity { UserName = "User1" } },
                    Categories = new List<CategoryEntity> { new CategoryEntity() }
                }
            };
            var spec = new GetTasksByUserNameSpecification(userName);

            // Act
            var results = spec.Evaluate(tasks.AsQueryable()).ToList();

            // Assert
            results.Should().HaveCount(2);
            results.Should().OnlyContain(t => t.AssignedUsers.Any(u => u.UserName == userName));
            results.Should().OnlyContain(t => t.Categories.Any());
        }
    }
}
