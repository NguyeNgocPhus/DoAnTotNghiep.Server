using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Application.Abstractions;

public interface IUserRepository
{
    public IQueryable<UserResponse> GetUsersAsync(CancellationToken cancellationToken = default);
    public Task<UserResponse?> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<bool?> RemoveRoleInUser(Guid id, CancellationToken cancellationToken);
}