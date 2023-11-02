namespace TaskHub.Dal.Entities
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
