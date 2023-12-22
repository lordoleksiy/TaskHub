using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Dal.Tests.Specifications
{
    [TestFixture]
    public class GetUserByNameOrEmailSpecificationTests
    {
        [Test]
        public void ShouldFilterUsersByNameOrEmail()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new UserEntity { UserName = "User1", Email = "user1@example.com" },
                new UserEntity { UserName = "User2", Email = "user2@example.com" },
                new UserEntity { UserName = "User3", Email = "user3@example.com" }
            };
            var spec = new GetUserByNameOrEmailSpecification("User1", "user2@example.com");

            // Act
            var results = spec.Evaluate(users.AsQueryable()).ToList();

            // Assert
            results.Should().HaveCount(2);
            results.Should().Contain(u => u.UserName == "User1");
            results.Should().Contain(u => u.Email == "user2@example.com");
        }
    }
}
