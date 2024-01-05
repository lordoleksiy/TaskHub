using Microsoft.AspNetCore.Identity;

namespace TaskHub.Dal.Entities
{
    public class UserEntity : IdentityUser<Guid>, IBaseEntity<Guid>
    {
        public ICollection<TaskEntity> Tasks { get; set; }  
        public ICollection<NotificationEntity> Reminders { get; set; }
    }
}
