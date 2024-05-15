using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DoAn.Infrastructure.Notification.Services;

public class NotificationService : INotificationService
{
     private readonly IRepositoryBase<Domain.Entities.Notification, int> _notificationRepository;
      private readonly IRepositoryBase<Domain.Entities.NotificationEvent, int> _notificationEventRepository;
     private readonly IUnitOfWork _unitOfWork;
     private readonly UserManager<AppUser> _userManager;
     private readonly ICurrentUserService _currentUserService;
     public NotificationService(IRepositoryBase<Domain.Entities.Notification, int> notificationRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IRepositoryBase<NotificationEvent, int> notificationEventRepository, UserManager<AppUser> userManager)
     {
          _notificationRepository = notificationRepository;
          _currentUserService = currentUserService;
          _unitOfWork = unitOfWork;
          _notificationEventRepository = notificationEventRepository;
          _userManager = userManager;
     }

     public async Task<bool> SendNotificationAsync(List<Guid> receiverIds, NotificationType type,Dictionary<string, string> fields, Guid? senderId = null)
     {
          try
          {
               senderId ??= Guid.Parse(_currentUserService.UserId);
               var typeNoti = await _notificationEventRepository.FindSingleAsync(x => x.Type == type);
               var text = typeNoti.Text;
               foreach (var field in fields)
               {
                    text = text.Replace($"[{field.Key}]", field.Value);
               }
               
               foreach (var receiverId in receiverIds)
               {
                    var n = new Domain.Entities.Notification()
                    {
                         Event = typeNoti.Type,
                         CreatedTime = DateTime.Now,
                         UserId = receiverId,
                         Read = false,
                         Title = typeNoti.Title,
                         Text = text,
                         CreatedBy = senderId.Value,
                    };
                    _notificationRepository.Add(n);
               }
               
               await _unitOfWork.SaveChangesAsync();
               return true;
          }
          catch (Exception e)
          {
               Console.WriteLine(e);
               throw;
          }
         
     }
}