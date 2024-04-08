using Microsoft.AspNetCore.Authorization;

namespace DoAn.Infrastructure.Abstractions;

public interface IAuthorizationHandler
{
    
    /// <summary>
    /// Makes a decision if authorization is allowed.
    /// </summary>
    /// <param name="context">The authorization information.</param>
    Task HandleAsync(AuthorizationHandlerContext context);
}