using DoAn.Application.Abstractions;
using DoAn.Shared.Services.V1.Notification;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class NotificationController: ApiControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(Common.Url.ADMIN.Notification.ViewList)]
    [Authorize]
    public async Task<ActionResult> Get([FromQuery ] GetNotificationQuery query, CancellationToken cancellationToken)
    {
        query.Size = 3;
        var result =await _mediator.Send(query, cancellationToken);
        if(!result.IsSuccess){}
        return Ok(result);
    }
    
    
    [HttpGet]
    [Route(Common.Url.ADMIN.Notification.CountUnread)]
    [Authorize]
    public async Task<ActionResult> GetUnreadNotification(CancellationToken cancellationToken)
    {
        var query = new GetCountUnreadNotificationQuery()
        {
            
        };
        var result =await _mediator.Send(query, cancellationToken);
        if(!result.IsSuccess){}
        return Ok(result);
    }
   
}