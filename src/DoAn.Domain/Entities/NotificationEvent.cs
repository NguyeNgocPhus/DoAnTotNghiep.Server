using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities;

public class NotificationEvent : Entity<int>
{
    public NotificationType Type { get; set; }
    public string Text { get; set; }
    public string Title { get; set; }
    
}

public enum NotificationType
{
    Upload,
    Approve,
    Reject,
    Finish
}