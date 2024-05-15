using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities;

public class Notification : Entity<int>
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Event { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public bool Read { get; set; }
}