using DoAn.Shared.Services.V1.ImportTemplate.Commands;
using DoAn.Shared.Services.V1.ImportTemplate.Queries;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;

public class ImportTemplateController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ImportTemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route(Common.Url.ADMIN.ImportTemplate.Create)]
    public async Task<ActionResult> Create([FromBody] CreateImportTemplateCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }

    [HttpPut] [Route(Common.Url.ADMIN.ImportTemplate.Update)]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateImportTemplateCommand request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpPost]
    [Route(Common.Url.ADMIN.ImportTemplate.ImportData)]
    public async Task<ActionResult> ImportData([FromBody] ImportDataCommand request,  CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpDelete]
    [Route(Common.Url.ADMIN.ImportTemplate.Delete)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteImportTemplateCommand()
        {
            Id = id
        };
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpGet]
    [Route(Common.Url.ADMIN.ImportTemplate.View)]
    public async Task<ActionResult> Get(Guid id,  CancellationToken cancellationToken)
    {
        var query = new GetImportTemplateQuery()
        {
            Id = id
        };
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpGet]
    [Route(Common.Url.ADMIN.ImportTemplate.ViewList)]
    public async Task<ActionResult> GetList([FromQuery] GetListImportTemplateQuery query, CancellationToken cancellationToken)
    {
       
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
}