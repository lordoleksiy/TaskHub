using Ardalis.Specification;
using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Repositories;
using TaskHub.Dal.Specification.UserSpecifications;
using TaskHub.Tests.TaskHub.Dal.Repositories;

namespace TaskHub.Dal.Tests
{
    [TestFixture]
    public class UserRepositoryTests : RepositoryTests<UserEntity, Guid, IUserRepository>
    {
        protected override IUserRepository CreateRepository(DataContext dataContext)
        {
            return new UserRepository(dataContext);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsUser()
        {
            // Arrange
            var existingUserId = Guid.NewGuid();
            var existingUser = new UserEntity { Id = existingUserId, UserName = "Test User", Email = "test@gmail.com" };
            _dataContext.Users.Add(existingUser);
            await _dataContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(existingUserId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(existingUser);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var nonExistingUserId = Guid.NewGuid();

            // Act
            var result = await _repository.GetByIdAsync(nonExistingUserId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task AddAsync_AddsUserToDatabase()
        {
            // Arrange
            var user = new UserEntity { Id = Guid.NewGuid(), UserName = "TestUser", Email = "test@example.com" };

            // Act
            await _repository.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            // Assert
            _dataContext.Users.Should().ContainEquivalentOf(user);
        }

        [Test]
        public async Task Remove_RemovesUserFromDatabase()
        {
            // Arrange
            var user = new UserEntity { Id = Guid.NewGuid(), UserName = "TestUser", Email = "test@example.com" };
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            // Act
            _repository.Remove(user);
            await _dataContext.SaveChangesAsync();

            // Assert
            _dataContext.Users.Should().NotContain(user);
        }

        [Test]
        public async Task Update_UpdatesUserInDatabase()
        {
            // Arrange
            var user = new UserEntity { Id = Guid.NewGuid(), UserName = "TestUser", Email = "test@example.com" };
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            // Act
            user.UserName = "UpdatedUser";
            _repository.Update(user);
            await _dataContext.SaveChangesAsync();

            // Assert
            _dataContext.Users.Single(u => u.Id == user.Id).UserName.Should().Be("UpdatedUser");
        }

        [Test]
        public async Task GetAsync_WithSpecification_ReturnsFilteredEntities()
        {
            // Arrange
            var entities = new List<UserEntity>
            {
                new UserEntity { Id = Guid.NewGuid(), UserName = "Test User 1", Email = "TestEmail1@gmail.com" },
                new UserEntity { Id = Guid.NewGuid(), UserName = "Test User 2", Email = "TestEmail2@gmail.com" },
                new UserEntity { Id = Guid.NewGuid(), UserName = "Test User 3", Email = "TestEmail3@gmail.com" }
            };
            await _dataContext.AddRangeAsync(entities);
            await _dataContext.SaveChangesAsync();

            var spec = new GetUserByUserNameSpecification("Test User 2");

            // Act
            var result = await _repository.GetAsync(spec);

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainEquivalentOf(entities[1]);
        }
    }
}
