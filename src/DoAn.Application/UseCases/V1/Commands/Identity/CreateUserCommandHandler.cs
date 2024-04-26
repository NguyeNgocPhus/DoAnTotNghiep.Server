using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
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
        var existUser = await _userManager.FindByEmailAsync(request.Email);
        if (existUser != null)
        {
            throw new DuplicateUserException("Duplicate user");
        }
        
        
        var passwordDefault = "123@123aA";
        var user = new AppUser()
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName, 
            LastName = request.LastName,
            FullName = request.FirstName + request.LastName,
            UserName = request.UserName,
            PositionId = Guid.NewGuid(),
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PasswordHash = _passwordGeneratorService.HashPassword(passwordDefault),
        };

        var userResult  = await _userManager.CreateAsync(user);
        if (userResult.Succeeded)
        {
            var currentUser = await _userManager.FindByIdAsync(user.Id.ToString()); 
            await _userManager.AddToRolesAsync(currentUser, request.Roles);
        }
        else
        { 
            throw new DuplicateUserException(string.Join(",", userResult.Errors.Select(x=>x.Description)));
        }
        
        return Result.Success(new UserResponse()
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FirstName + user.LastName ,
            Roles = request.Roles
        });
    }
}