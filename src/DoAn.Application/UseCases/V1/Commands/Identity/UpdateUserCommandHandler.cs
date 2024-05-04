using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Application.UseCases.V1.Commands.Identity
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordGeneratorService _passwordGeneratorService;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager,
            IPasswordGeneratorService passwordGeneratorService)
        {
            _userManager = userManager;
            _passwordGeneratorService = passwordGeneratorService;
        }
        public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existUserByName = await _userManager.FindByNameAsync(request.UserName);
            if (existUserByName == null)
            {
                throw new DuplicateUserException("not exist user");
            }
            var existUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existUserByEmail != null && existUserByEmail.Id.ToString() != request.Id)
            {
                throw new DuplicateUserException("Have user use this email");
            }
            existUserByEmail.FirstName = request.FirstName;
            existUserByEmail.LastName = request.LastName;
            existUserByEmail.FullName = request.FirstName + request.LastName;
            existUserByEmail.UserName = request.UserName;
            existUserByEmail.PhoneNumber = request.PhoneNumber;
            existUserByEmail.Email = request.Email;
            var userResult = await _userManager.UpdateAsync(existUserByEmail);
            if (!userResult.Succeeded)
            {
                throw new DuplicateUserException(string.Join(",", userResult.Errors.Select(x => x.Description)));
            }
            var oldRoleName = (await _userManager.GetRolesAsync(existUserByEmail)).ToList();
            var deleteRole = oldRoleName.Where(r => !request.Roles.Contains(r));
            var addRoles = request.Roles.Where(r => !oldRoleName.Contains(r));
            var resultDelete = await _userManager.RemoveFromRolesAsync(existUserByEmail, deleteRole);
            if (!resultDelete.Succeeded)
            {
                throw new DuplicateUserException(string.Join(",", resultDelete.Errors.Select(x => x.Description)));
            }
            var resultAdd = await _userManager.AddToRolesAsync(existUserByEmail, addRoles);
            if (!resultAdd.Succeeded)
            {
                throw new DuplicateUserException(string.Join(",", resultAdd.Errors.Select(x => x.Description)));
            }

            return Result.Success(new UserResponse()
            {
                Id = existUserByEmail.Id,
                Email = existUserByEmail.Email,
                PhoneNumber = existUserByEmail.PhoneNumber,
                UserName = existUserByEmail.UserName,
                FirstName = existUserByEmail.FirstName,
                LastName = existUserByEmail.LastName,
                FullName = existUserByEmail.FirstName + existUserByEmail.LastName,
                Roles = request.Roles
            });
        }
    }
}
