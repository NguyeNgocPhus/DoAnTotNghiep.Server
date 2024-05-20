using ICommand = DoAn.Shared.Abstractions.Messages.ICommand;

namespace DoAn.Shared.Services.V1.Notification;

public class UpdateNotificationCommand : ICommand
{
    public int Id { get; set; }
}