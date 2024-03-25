using DoAn.Shared.Services.V1.Workflow.Commands;
using Elsa.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn.API.Controllers;
 
public class WorkflowController : ApiControllerBase
{

    private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
    private readonly IMediator _mediator;
    public WorkflowController(IWorkflowDefinitionStore workflowDefinitionStore, IMediator mediator)
    {
        _workflowDefinitionStore = workflowDefinitionStore;
        _mediator = mediator;
    }
    
    [HttpPost]
    [Route("Create")]
    // [Authorize]
    public async Task<ActionResult> CreateWorkflow([FromBody] CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
            
        }
        return Ok(result.Value);
    }
}