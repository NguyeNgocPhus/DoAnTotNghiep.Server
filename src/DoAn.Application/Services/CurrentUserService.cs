using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace DoAn.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtTokenService _jwtService;

    public CurrentUserService(
        
        IJwtTokenService jwtService, IHttpContextAccessor httpContextAccessor)
    {
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId
    {
        get
        {
            string jwtToken = GetJwtToken();
            if (jwtToken == null || jwtToken.IsMissing())
                throw new UnauthorizedJwtException(nameof(CurrentUserService), "You are not logged in yet");

            return _jwtService.DecodeAccessToken(jwtToken).Claims.First(c => c.Type == "Id").Value.ToString() ??
                   throw new UnauthorizedJwtException(nameof(CurrentUserService), "You are not logged in yet");
        }
    }

    

    public string Email
    {
        get
        {
            string jwtToken = GetJwtToken();
            if (jwtToken == null || jwtToken.IsMissing())
                throw new UnauthorizedJwtException(nameof(CurrentUserService), "You are not logged in yet");

            return _jwtService.DecodeAccessToken(jwtToken).Claims.First(c => c.Type == "Email").Value ??
                   throw new UnauthorizedJwtException(nameof(CurrentUserService), "You are not logged in yet");
        }
    }


    public bool HasRole(string role)
    {
        string jwtToken = GetJwtToken();
        if (jwtToken == null || jwtToken.IsMissing())
            throw new UnauthorizedJwtException(nameof(CurrentUserService), "You are not logged in yet");
        return _jwtService.DecodeAccessToken(jwtToken).Claims
            .Any(claim => claim.Type == nameof(role) && claim.Value == role);
    }

    public string? GetJwtToken()
    {
        HttpContext httpContext = _httpContextAccessor.HttpContext;
        return (httpContext != null
            ? httpContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization")
            : new KeyValuePair<string, StringValues>?())?.Value.ToString().Replace("Bearer ", "");
    }
}