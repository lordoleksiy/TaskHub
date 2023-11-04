namespace TaskHub.Dal.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository UserRepository { get; }
        ITaskRepository TaskRepository { get; }
        IReminderRepository ReminderRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task Commit();
    }
}
