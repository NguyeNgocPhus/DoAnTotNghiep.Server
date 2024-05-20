using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;

namespace DoAn.Shared.Services.V1.Notification;

public class GetNotificationQuery: PaginationBaseRequest, IQuery<List<NotificationResponse>>
{
    
}