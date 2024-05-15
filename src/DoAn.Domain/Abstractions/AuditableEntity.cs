namespace DoAn.Domain.Abstractions;

public class AuditableEntity<T> : Entity<T>, IAuditableEntity<T>
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public T CreatedBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public T? UpdatedBy { get; set; }
}