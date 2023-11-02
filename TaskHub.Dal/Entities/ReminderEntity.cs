namespace TaskHub.Dal.Entities
{
    public class ReminderEntity<TKey> : IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public TKey UserId { get; set; }
        public UserEntity<TKey> User { get; set; }
        public TKey TaskId { get; set; }
        public TaskEntity<TKey> Task { get; set; }
        public DateTime ReminderTime { get; set; }
        public DateTime DueTime { get; set; }
    }
}
