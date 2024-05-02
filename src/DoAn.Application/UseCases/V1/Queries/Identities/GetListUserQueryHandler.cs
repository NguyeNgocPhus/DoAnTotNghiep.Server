using System.Linq.Expressions;
using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Queries;
using DoAn.Shared.Services.V1.Identity.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.Identities;

public class GetListUserQueryHandler : IQueryHandler<GetListUserQuery, PagedResult<UserResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    public GetListUserQueryHandler(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public async Task<Result<PagedResult<UserResponse>>> Handle(GetListUserQuery request,
        CancellationToken cancellationToken)
    {
        var query = _userRepository.GetUsersAsync();
        
        query = query.Where(x => x.UserName.Contains(request.Keyword) || x.Email.Contains(request.Keyword));

        query = request.OrderByDesc
            ? query.OrderByDescending(GetSortProperty(request))
            : query.OrderBy(GetSortProperty(request));

        var users = await PagedResult<UserResponse>.CreateAsync(query, 1, 100);

        return Result.Success(users);
    }
    private static Expression<Func<UserResponse, object>> GetSortProperty(GetListUserQuery request)
        => request.OrderBy?.ToLower() switch
        {
           
            _ => product => product.Id
            //_ => product => product.CreatedDate // Default Sort Descending on CreatedDate column
        };
}