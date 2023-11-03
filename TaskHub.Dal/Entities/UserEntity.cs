using Microsoft.AspNetCore.Identity;

namespace TaskHub.Dal.Entities
{
    public class UserEntity<TKey> : IdentityUser<TKey>, IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        override public TKey Id { get; set; }
        public ICollection<TaskEntity<TKey>> Tasks { get; set; }  
        public ICollection<ReminderEntity<TKey>> Reminders { get; set; }
    }
}
