namespace TaskHub.Common.DTO.Task
{
    public record NewTaskDTO
    {
        public string Title { get; init; }
        public string DueDate { get; init; }
        public string? Description { get; init; }
        public ICollection<string>? Categories { get; init; }
        public ICollection<string> AssignedUserNames { get; set; }
        public string? ParentTaskId { get; init; }
    }
}
