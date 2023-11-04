using Microsoft.AspNetCore.Identity;

namespace TaskHub.Dal.Entities
{
    public class UserEntity : IdentityUser<Guid>, IBaseEntity<Guid>
    {
        override public Guid Id { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }  
        public ICollection<ReminderEntity> Reminders { get; set; }
    }
}
