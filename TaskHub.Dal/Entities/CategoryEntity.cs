namespace TaskHub.Dal.Entities
{
    public class CategoryEntity : IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; }
    }
}
