using AutoMapper;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
      
        CreateMap<AppUser, UserResponse>();
    }
}