namespace TaskHub.Dal.Entities
{
    public class CategoryEntity<TKey> : IBaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<TaskEntity<TKey>> Tasks { get; set; }
    }
}
