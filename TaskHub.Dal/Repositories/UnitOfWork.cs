using TaskHub.Dal.Context;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        private bool _disposed;
        private ICategoryRepository _categoryRepository;
        private IReminderRepository _reminderRepository;
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
        public IReminderRepository ReminderRepository
        {
            get
            {
                _reminderRepository ??= new ReminderRepository(_dbContext);
                return _reminderRepository;
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
