using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Dal.Tests.Specifications
{
    [TestFixture]
    public class GetUsersByNamesSpecificationTests
    {
        [Test]
        public void ShouldFilterUsersByUserNames()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new UserEntity { UserName = "User1" },
                new UserEntity { UserName = "User2" },
                new UserEntity { UserName = "User3" }
            };
            var userNames = new List<string> { "User1", "User3" };
            var spec = new GetUsersByNamesSpecification(userNames);

            // Act
            var results = spec.Evaluate(users.AsQueryable()).ToList();

            // Assert
            results.Should().HaveCount(2);
            results.Should().Contain(u => u.UserName == "User1");
            results.Should().Contain(u => u.UserName == "User3");
        }
    }
}
