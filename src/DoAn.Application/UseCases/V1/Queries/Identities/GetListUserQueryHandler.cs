using AutoMapper;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Queries;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.Identities;

public class GetListUserQueryHandler : IQueryHandler<GetListUserQuery, List<UserResponse>>
{

    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;

    public GetListUserQueryHandler(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<Result<List<UserResponse>>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
    {
        
        var users = await _userManager.Users.Where(x => !x.IsDirector).ToListAsync(cancellationToken)
            ;
        var result = new List<UserResponse>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userMapper = _mapper.Map<UserResponse>(user);
            userMapper.Roles = roles;
            result.Add(userMapper);
        }
        
        


       
        return Result.Success(result.ToList());
    }
}