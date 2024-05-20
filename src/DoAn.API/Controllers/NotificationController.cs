using DoAn.Shared.Services.V1.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class NotificationController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(Common.Url.ADMIN.Notification.ViewList)]
    public async Task<ActionResult> Get([FromQuery] GetNotificationQuery query, CancellationToken cancellationToken)
    {
        query.Size = 3;
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }


    [HttpGet]
    [Route(Common.Url.ADMIN.Notification.CountUnread)]
    public async Task<ActionResult> GetUnreadNotification(CancellationToken cancellationToken)
    {
        var query = new GetCountUnreadNotificationQuery()
        {
        };
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }

    [HttpPut]
    [Route(Common.Url.ADMIN.Notification.Update)]
    public async Task<ActionResult> Update([FromBody] UpdateNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }
}