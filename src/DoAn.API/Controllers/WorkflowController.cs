using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;
 
public class WorkflowController : ApiControllerBase
{
    private readonly IMediator _mediator;
    public WorkflowController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route(Common.Url.ADMIN.Workflow.CreateWorkflowDefinition)]
    public async Task<ActionResult> CreateWorkflowDefinition([FromBody] CreateWorkflowDefinitionCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpPut]
    [Route(Common.Url.ADMIN.Workflow.UpdateWorkflowDefinition)]
    public async Task<ActionResult> UpdateWorkflowDefinition(string id, [FromBody] UpdateWorkflowDefinitionCommand request, CancellationToken cancellationToken)
    {
        request.WorkflowDefinitionId = id;
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpGet]
    [Route(Common.Url.ADMIN.Workflow.ViewListWorkflowDefinition)]
    public async Task<ActionResult> GetListWorkflowDefinition(CancellationToken cancellationToken)
    {
        var query = new GetListWorkflowDefinitionQuery();
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpGet]
    [Route(Common.Url.ADMIN.Workflow.ViewWorkflowDefinition)]
    public async Task<ActionResult> GetWorkflowDefinition(string id, CancellationToken cancellationToken)
    {
        var query = new GetWorkflowDefinitionQuery()
        {
            DefinitionId = id
        };
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
    [HttpDelete]
    [Route(Common.Url.ADMIN.Workflow.DeleteWorkflowDefinition)]
    public async Task<ActionResult> DeleteWorkflowDefinition(string Id,  CancellationToken cancellationToken)
    {
        var command = new DeleteWorkflowDefinitionCommand()
        {
            DefinitionId = Id
        };
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result);
    }
}