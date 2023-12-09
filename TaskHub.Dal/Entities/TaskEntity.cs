using TaskHub.Common.Enums;

namespace TaskHub.Dal.Entities
{
    public class TaskEntity: IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatusCode Status { get; set; }
        public Guid? ParentTaskId { get; set; }
        public TaskEntity? ParentTask { get; set; }
        public ICollection<TaskEntity>? Subtasks { get; set; }
        public ICollection<UserEntity> AssignedUsers { get; set; }
        public ICollection<CategoryEntity>? Categories { get; set; }
        public ICollection<ReminderEntity>? Reminders { get; set; }
    }
}
