using TaskHub.Common.Enums;

namespace TaskHub.Common.DTO.Task
{
    public class UpdateTaskDTO
    {
        public string Id { get; set; }
        public string Title { get; init; }
        public string DueDate { get; init; }
        public string? Description { get; init; }
        public ICollection<string>? Categories { get; init; }
        public ICollection<string>? AssignedUserNames { get; init; }
        public TaskStatusCode Status { get; init; }
    }
}
