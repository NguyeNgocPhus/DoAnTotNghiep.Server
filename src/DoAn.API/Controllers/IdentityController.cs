using DoAn.Shared.Services.V1.Identity.Commands;
using DoAn.Shared.Services.V1.Identity.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class IdentityController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route(Common.Url.ADMIN.Identity.Login)]
    public async Task<ActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route(Common.Url.ADMIN.Identity.User.ViewList)]
    public async Task<ActionResult> GetListUser(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetListUserQuery(), cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    
    [HttpPost]
    [Route(Common.Url.ADMIN.Identity.User.Create)]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
}