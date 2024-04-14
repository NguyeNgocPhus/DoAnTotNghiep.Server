using DoAn.Shared.Services.V1.Roles.Queries;
using DoAn.Shared.Services.V1.Workflow.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class RoleController: ApiControllerBase
{
    private readonly IMediator _mediator;
    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route(Common.Url.ADMIN.Roles.ViewList)]
    public async Task<ActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRolesQuery(), cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    
}