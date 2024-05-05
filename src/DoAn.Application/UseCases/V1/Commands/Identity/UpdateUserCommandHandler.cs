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
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Commands.Identity
{
    public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordGeneratorService _passwordGeneratorService;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(UserManager<AppUser> userManager,
            IPasswordGeneratorService passwordGeneratorService, IMapper mapper, IUserRepository userRepository, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _passwordGeneratorService = passwordGeneratorService;
            _mapper = mapper;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }
        public async Task<Result<UserResponse>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(command.Id);
            if (user == null)
            {
                throw new DuplicateUserException("not exist user");
            }
            
            var existUserByEmail = await _userManager.FindByEmailAsync(command.Email);
            if (existUserByEmail != null && existUserByEmail.Id.ToString() != command.Id)
            {
                throw new DuplicateUserException("Have user use this email");
            }
            // check valid roles
            var roles =  await _roleManager.Roles.Where(x=>command.Roles.Contains(x.RoleCode)).ToListAsync(cancellationToken);
            if (roles.Count != command.Roles.Count)
            {
                
            }
            // remove old role
            await _userRepository.RemoveRoleInUser(user.Id, cancellationToken);
           
            user.FirstName = command.UserName;
            user.LastName = command.UserName;
            user.FullName = command.UserName ;
            user.UserName = command.UserName;
            user.PhoneNumber = command.PhoneNumber;
            user.Email = command.Email;
            user.UserRoles = roles.Select(x => new IdentityUserRole<Guid>()
            {
                UserId = user.Id,
                RoleId = x.Id
            }).ToList();
            

            var userResult = await _userManager.UpdateAsync(user);
            if (!userResult.Succeeded)
            {
                throw new DuplicateUserException(string.Join(",", userResult.Errors.Select(x => x.Description)));
            }
            

            var result = _mapper.Map<UserResponse>(command);
            return Result.Success(result);
        }
    }
}
