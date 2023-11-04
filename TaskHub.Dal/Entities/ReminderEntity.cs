namespace TaskHub.Dal.Entities
{
    public class ReminderEntity : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
        public Guid TaskId { get; set; }
        public TaskEntity Task { get; set; }
        public DateTime ReminderTime { get; set; }
        public DateTime DueTime { get; set; }
    }
}
