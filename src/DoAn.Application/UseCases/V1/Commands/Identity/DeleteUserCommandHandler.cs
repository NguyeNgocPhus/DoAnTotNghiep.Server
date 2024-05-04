using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Application.UseCases.V1.Commands.Identity
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordGeneratorService _passwordGeneratorService;

        public DeleteUserCommandHandler(UserManager<AppUser> userManager,
            IPasswordGeneratorService passwordGeneratorService)
        {
            _userManager = userManager;
            _passwordGeneratorService = passwordGeneratorService;
        }
        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existUser = await _userManager.FindByIdAsync(request.Id);
            if (existUser == null)
            {
                throw new DuplicateUserException("Not find user");
            }
            existUser.IsDeleted = true;
            var userResult = await _userManager.UpdateAsync(existUser);
            if (!userResult.Succeeded)
            {
                throw new DuplicateUserException(string.Join(",", userResult.Errors.Select(x => x.Description)));
            }
            return Result.Success();
        }
    }
}
