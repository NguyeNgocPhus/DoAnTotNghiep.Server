using System.Security.Claims;
using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Identity;

namespace DoAn.Application.UseCases.V1.Commands.Identity;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ICacheService _cacheService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordGeneratorService _passwordGeneratorService;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUserRepository _userRepository;
    
    
    public LoginCommandHandler(IJwtTokenService jwtTokenService, ICacheService cacheService, UserManager<AppUser> userManager, IPasswordGeneratorService passwordGeneratorService, RoleManager<AppRole> roleManager, IUserRepository userRepository)
    {
        _jwtTokenService = jwtTokenService;
        _cacheService = cacheService;
        _userManager = userManager;
        _passwordGeneratorService = passwordGeneratorService;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new LoginFailException("login fail", "email or password is not correct");
        }

        if (!_passwordGeneratorService.VerifyHashPassword(user.PasswordHash, request.Password)
           )
        {
            throw new LoginFailException("login fail", "email or password is not correct");
        }
        var roles =await  _userRepository.GetRoleCodeByUser(user.Id, cancellationToken);

        // throw new TestException("Not Found", "okeokeo");
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Email, request.Email)
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim("Roles", role));
        }
        if (user.IsDirector is true)
        {
            claims.Add(new Claim("IsDirector", "true"));
        }
        
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