using DoAn.Shared.Services.V1.ImportHistory.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class ImportHistoryController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ImportHistoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route(Common.Url.ADMIN.ImportHistory.ViewList)]
    public async Task<ActionResult> GetList([FromQuery] GetListImportHistoryQuery request,
        CancellationToken cancellationToken)
    {
        
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }
}