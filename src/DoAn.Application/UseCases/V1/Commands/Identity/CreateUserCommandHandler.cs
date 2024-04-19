using DoAn.Application.Abstractions;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Identity;

namespace DoAn.Application.UseCases.V1.Commands.Identity;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordGeneratorService _passwordGeneratorService;

    public CreateUserCommandHandler(UserManager<AppUser> userManager,
        IPasswordGeneratorService passwordGeneratorService)
    {
        _userManager = userManager;
        _passwordGeneratorService = passwordGeneratorService;
    }

    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var passwordDefault = "123@123aA";
        var user = new AppUser()
        {
            UserName = request.Name,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PasswordHash = _passwordGeneratorService.HashPassword(passwordDefault),
        };
        await _userManager.CreateAsync(user);
        return Result.Success(new UserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Name = user.UserName
        });
    }
}