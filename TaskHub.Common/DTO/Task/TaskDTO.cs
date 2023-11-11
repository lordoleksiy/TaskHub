using TaskHub.Common.Enums;

namespace TaskHub.Common.DTO.Task
{
    public record TaskDTO
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime DueDate { get; set; }
        public TaskStatusCode Status { get; set; }
        public ICollection<string> AssignedUserNames { get; set; }
        public ICollection<string> Categories { get; set; }
    }
}
