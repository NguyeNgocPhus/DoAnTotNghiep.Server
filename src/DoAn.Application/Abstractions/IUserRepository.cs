using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Application.Abstractions;

public interface IUserRepository 
{
    public Task<IEnumerable<UserResponse>> GetUserHasRole(Guid roleId, CancellationToken cancellationToken = default);
    public IQueryable<UserResponse> GetUsersAsync(CancellationToken cancellationToken = default);
    public Task<UserResponse?> GetUserByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task<bool?> RemoveRoleInUser(Guid id, CancellationToken cancellationToken);
    Task<List<string>> GetRoleCodeByUser(Guid id, CancellationToken cancellationToken = default);
    Task<List<string>> GetRoleIdsInUser(Guid id, CancellationToken cancellationToken = default);
}