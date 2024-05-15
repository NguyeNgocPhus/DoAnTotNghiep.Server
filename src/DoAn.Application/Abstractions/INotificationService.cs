using DoAn.Domain.Entities;

namespace DoAn.Application.Abstractions;

public interface INotificationService
{
    Task<bool> SendNotificationAsync(List<Guid> receiver, NotificationType type, Dictionary<string, string> fields,
        Guid? senderId = null);
}