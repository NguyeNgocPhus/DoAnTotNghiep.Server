namespace DoAn.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public T CreatedBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public T? UpdatedBy { get; set; }
}