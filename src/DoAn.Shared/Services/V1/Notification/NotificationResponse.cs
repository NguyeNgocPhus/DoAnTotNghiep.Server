namespace DoAn.Shared.Services.V1.Notification;

public class NotificationResponse
{
    public int Id { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid UserId { get; set; }
    public int Event { get; set; }
    public bool Read { get; set; }
    public string CreatedByName { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    
}