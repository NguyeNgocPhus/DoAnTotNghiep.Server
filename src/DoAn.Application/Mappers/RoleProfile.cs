using AutoMapper;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Services.V1.Roles.Responses;

namespace DoAn.Application.Mappers;

public class RoleProfile: Profile
{
    public RoleProfile()
    {
      
        CreateMap<AppRole, RoleResponse>();
    }
}