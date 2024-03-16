namespace DoAn.Domain.Abstractions;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public bool IsDeleted { get; set; }
}