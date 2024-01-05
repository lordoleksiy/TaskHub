using TaskHub.Dal.Entities;
using TaskHub.Dal.Specification.UserSpecifications;

namespace TaskHub.Dal.Tests.Specifications
{
    [TestFixture]
    public class GetUserByUserNameSpecificationTests
    {
        [Test]
        public void ShouldFilterUsersByUserName()
        {
            // Arrange
            var users = new List<UserEntity>
            {
                new UserEntity { UserName = "User1" },
                new UserEntity { UserName = "User2" },
                new UserEntity { UserName = "User3" }
            };
            var spec = new GetUserByUserNameSpecification("User1");

            // Act
            var results = spec.Evaluate(users.AsQueryable()).ToList();

            // Assert
            results.Should().HaveCount(1);
            results.Should().Contain(u => u.UserName == "User1");
        }
    }
}
