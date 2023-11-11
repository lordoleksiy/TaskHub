using TaskHub.Common.Enums;

namespace TaskHub.Dal.Entities
{
    public class SubTaskEntity : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ParentTaskId { get; set; }
        public TaskEntity ParentTask { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public SubTaskStatusCode Status { get; set; }
        public UserEntity Performer { get; set; }
        public Guid PerformerId { get; set; }

    }
}
