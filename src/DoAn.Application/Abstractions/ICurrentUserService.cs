namespace DoAn.Application.Abstractions;

public interface ICurrentUserService
{
    string UserId { get; }
    string Email { get; }
    bool HasRole(string role);
    string? GetJwtToken();
}