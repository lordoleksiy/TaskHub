using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Tests.TaskHub.Dal.Repositories
{
    public abstract class RepositoryTests<TEntity, TKey, TRepository> where TEntity : class, IBaseEntity<TKey> where TRepository : IGenericRepository<TEntity, TKey>
    {
        protected DataContext _dataContext;
        protected TRepository _repository;

        [SetUp]
        public virtual void Setup()
        {
            _dataContext = CreateFakeDataContext();
            _repository = CreateRepository(_dataContext);
        }

        protected abstract TRepository CreateRepository(DataContext dataContext);

        private static DataContext CreateFakeDataContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "FakeDataContext")
                .Options;

            var dbContext = new DataContext(options);

            return dbContext;
        }
    }
}
