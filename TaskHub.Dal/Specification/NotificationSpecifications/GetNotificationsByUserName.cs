using Ardalis.Specification;
using TaskHub.Dal.Entities;

namespace TaskHub.Dal.Specification.NotificationSpecifications
{
    public class GetNotificationsByUserName : Specification<NotificationEntity>
    {
        public GetNotificationsByUserName(string userName) 
        {
            Query
                .Where(n => n.User.UserName == userName)
                .Include(n => n.Task);
        }
    }
}
