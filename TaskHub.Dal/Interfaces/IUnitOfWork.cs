namespace TaskHub.Dal.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository UserRepository { get; }
        ITaskRepository TaskRepository { get; }
        INotificationRepository NotificationRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task Commit();
    }
}
