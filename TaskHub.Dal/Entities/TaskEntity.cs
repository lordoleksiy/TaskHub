namespace TaskHub.Dal.Entities
{
    public class TaskEntity<TKey>: IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public TKey? ParentTaskId { get; set; }
        public TaskEntity<TKey>? ParentTask { get; set; }
        public ICollection<TaskEntity<TKey>>? Subtasks { get; set; }
        public ICollection<UserEntity<TKey>> AssignedUsers { get; set; }
        public ICollection<CategoryEntity<TKey>> Categories { get; set; }
    }
}
