using TaskHub.Dal.Context;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        private bool _disposed;
        private ICategoryRepository _categoryRepository;
        private INotificationRepository _notificationRepository;
        private ITaskRepository _taskRespository;
        private IUserRepository _userRepository;
        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ICategoryRepository CategoryRepository 
        { 
            get 
            {
                _categoryRepository ??= new CategoryRepository(_dbContext);
                return _categoryRepository; 
            } 
        }
        public INotificationRepository NotificationRepository
        {
            get
            {
                _notificationRepository ??= new NotificationRepository(_dbContext);
                return _notificationRepository;
            }
        }
        public ITaskRepository TaskRepository
        {
            get
            {
                _taskRespository ??= new TaskRespository(_dbContext);
                return _taskRespository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_dbContext);
                return _userRepository;
            }
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
