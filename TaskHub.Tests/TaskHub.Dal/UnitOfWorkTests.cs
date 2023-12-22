using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using TaskHub.Dal.Repositories;

namespace TaskHub.Dal.Tests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private DataContext _dataContext;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            _dataContext = CreateFakeDataContext();
            _unitOfWork = new UnitOfWork(_dataContext);
        }

        private static DataContext CreateFakeDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeDataContext")
                .Options;

            var dbContext = new DataContext(options);

            return dbContext;
        }

        [Test]
        public void CategoryRepository_ShouldNotBeNull()
        {
            // Assert
            _unitOfWork.CategoryRepository.Should().NotBeNull();
        }

        [Test]
        public void NotificationRepository_ShouldNotBeNull()
        {
            // Assert
            _unitOfWork.NotificationRepository.Should().NotBeNull();
        }

        [Test]
        public void TaskRepository_ShouldNotBeNull()
        {
            // Assert
            _unitOfWork.TaskRepository.Should().NotBeNull();
        }

        [Test]
        public void UserRepository_ShouldNotBeNull()
        {
            // Assert
            _unitOfWork.UserRepository.Should().NotBeNull();
        }

        [Test]
        public void Dispose_DisposesDbContext()
        {
            // Act
            _unitOfWork.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => _dataContext.Users.Count());
        }

        [Test]
        public async Task Commit_ShouldSaveChangesToDatabase()
        {
            // Arrange
            var initialCount = _dataContext.Users.Count();
            var newUser = new UserEntity { Id = Guid.NewGuid(), UserName = "TestUser", Email = "test@example.com" };

            // Act
            await _unitOfWork.UserRepository.AddAsync(newUser);
            await _unitOfWork.Commit(); // Commit changes to the database

            // Assert
            _dataContext.Users.Count().Should().Be(initialCount + 1); 
            _dataContext.Users.Should().ContainEquivalentOf(newUser);
        }
    }
}
