using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Interfaces
{
    public interface INotificationRepository: IGenericRepository<NotificationEntity, Guid>
    {
    }
}
