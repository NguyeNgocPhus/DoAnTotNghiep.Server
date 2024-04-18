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
    public async Task<ActionResult> GetList([FromQuery] Guid? importTemplateId, [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var query = new GetListImportHistoryQuery()
        {
            ImportTemplateId = importTemplateId,
            Status = status
        };
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }
}