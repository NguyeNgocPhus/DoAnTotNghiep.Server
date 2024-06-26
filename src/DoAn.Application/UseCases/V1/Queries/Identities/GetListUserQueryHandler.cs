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

        if (!string.IsNullOrEmpty(request.Email))
        {
            query = query.Where(x => x.Email.Contains(request.Email));
        }
        if (!string.IsNullOrEmpty(request.Name ))
        {
            query = query.Where(x => x.UserName.Contains(request.Name));
        }
        if (!string.IsNullOrEmpty(request.PhoneNumber ))
        {
            query = query.Where(x => x.PhoneNumber.Contains(request.PhoneNumber));
        }
        if (request.Roles.Count > 0)
        {
            query = query.Where(x => x.Roles.Any(r=>request.Roles.Contains(r)));
        }

        query = request.OrderByDesc
            ? query.OrderByDescending(GetSortProperty(request))
            : query.OrderBy(GetSortProperty(request));

        var users = await PagedResult<UserResponse>.CreateAsync(query, request.Page, request.Size);

        return Result.Success(users);
    }
    private static Expression<Func<UserResponse, object>> GetSortProperty(GetListUserQuery request)
        => request.OrderBy?.ToLower() switch
        {
           
            _ => product => product.Id
            //_ => product => product.CreatedDate // Default Sort Descending on CreatedDate column
        };
}