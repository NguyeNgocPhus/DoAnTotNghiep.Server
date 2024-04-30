using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Application.Abstractions;

public interface IUserRepository
{
    public IQueryable<UserResponse> GetUsersAsync();
}