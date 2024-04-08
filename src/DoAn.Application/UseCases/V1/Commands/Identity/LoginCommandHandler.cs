using System.Security.Claims;
using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Application.UseCases.V1.Commands.Identity;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    

    public LoginCommandHandler(IJwtTokenService jwtTokenService, ICacheService cacheService)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // throw new TestException("Not Found", "okeokeo");
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, request.Email),
            new(ClaimTypes.Role, "Senior .NET Leader")
        };
        
        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var login = new LoginResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        await _cacheService.SetAsync(request.Email, login, cancellationToken);
        return Result.Success(login);
    }
}