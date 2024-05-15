using AutoMapper;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;
using DoAn.Shared.Services.V1.Notification;

namespace DoAn.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
      
        CreateMap<AppUser, UserResponse>();
        CreateMap<UpdateUserCommand, UserResponse>();
        CreateMap<Notification, NotificationResponse>();
    }
}