using AutoMapper;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Roles.Queries;
using DoAn.Shared.Services.V1.Roles.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DoAn.Application.UseCases.V1.Queries.Roles;

public class GetListRoleQueryHandler : IQueryHandler<GetRolesQuery, List<RoleResponse>>
{
    private readonly RoleManager<AppRole> _repository;
    private readonly IMapper _mapper;

    public GetListRoleQueryHandler(RoleManager<AppRole> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public async Task<Result<List<RoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _repository.Roles.ToListAsync(cancellationToken);
        return Result.Success(_mapper.Map<List<RoleResponse>>(roles));
    }
}