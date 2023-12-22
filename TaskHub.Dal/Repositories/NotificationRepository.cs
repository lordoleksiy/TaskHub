using TaskHub.Dal.Context;
using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Repositories
{
    public class NotificationRepository : GenericRepository<NotificationEntity, Guid>, INotificationRepository
    {
        public NotificationRepository(DataContext dbContext) : base(dbContext)
        {
        }
    }
}
