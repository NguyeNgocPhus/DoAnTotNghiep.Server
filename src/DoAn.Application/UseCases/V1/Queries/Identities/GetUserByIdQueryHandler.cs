using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Queries;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Identity;

namespace DoAn.Application.UseCases.V1.Queries.Identities;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.Id, cancellationToken);

  
        return Result.Success(user);
    }
    
}