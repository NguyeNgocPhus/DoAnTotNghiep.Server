using System.Runtime.InteropServices;
using DoAn.Application.Abstractions;
using DoAn.Application.DTOs.Workflow;
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
    private readonly IWorkflowLaunchpadService _WorkflowLaunchpadService;

    public WorkflowController(IMediator mediator, IWorkflowLaunchpadService WorkflowLaunchpadService)
    {
        _mediator = mediator;
        _WorkflowLaunchpadService = WorkflowLaunchpadService;
    }

    [HttpPost]
    // [Authorize("AtLeast21")]
    [Route(Common.Url.ADMIN.Workflow.TestStartWorkflow)]
    public async Task<ActionResult> StartWorkflow([FromBody] ExecuteFileUpdateDto request,
        CancellationToken cancellationToken)
    {
        var result = await _WorkflowLaunchpadService.StartWorkflowsAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Route(Common.Url.ADMIN.Workflow.GetCurrentStepWorkflow)]
    public async Task<ActionResult> GetCurrentStepWorkflow(Guid fileId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentStepWorkflowInstanceQuery()
        {
            FileId = fileId
        }, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }
    [HttpGet]
    [Route(Common.Url.ADMIN.Workflow.GetWorkflowActivity)]
    public async Task<ActionResult> GetWorkflowActivity(Guid fileId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetWorkflowActivityQuery()
        {
            FileId = fileId
        }, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }
    
    [HttpGet]
    [Route(Common.Url.ADMIN.Workflow.GetWorkflowHistory)]
    public async Task<ActionResult> GetWorkflowHistory(Guid fileId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentStepWorkflowInstanceQuery()
        {
            FileId = fileId
        }, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }

    [HttpPost]
    // [Authorize("AtLeast21")]
    [Route(Common.Url.ADMIN.Workflow.ExecuteWorkflow)]
    public async Task<ActionResult> ExecuteWorkflow([FromBody] ExecuteWorkflowCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }


    [HttpPost]
    // [Authorize("AtLeast21")]
    [Route(Common.Url.ADMIN.Workflow.CreateWorkflowDefinition)]
    public async Task<ActionResult> CreateWorkflowDefinition([FromBody] CreateWorkflowDefinitionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }

    [HttpPut]
    [Route(Common.Url.ADMIN.Workflow.UpdateWorkflowDefinition)]
    public async Task<ActionResult> UpdateWorkflowDefinition(string id,
        [FromBody] UpdateWorkflowDefinitionCommand request, CancellationToken cancellationToken)
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
    public async Task<ActionResult> DeleteWorkflowDefinition(string Id, CancellationToken cancellationToken)
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

    [HttpGet]
    [Route(Common.Url.ADMIN.Workflow.ViewNode)]
    public async Task<ActionResult> ViewNodeDetail(string id, string type, CancellationToken cancellationToken)
    {
        var query = new GetNodeDefinitionQuery()
        {
            DefinitionId = id,
            Type = type
        };
        var result = await _mediator.Send(query, cancellationToken);
        if (!result.IsSuccess)
        {
        }

        return Ok(result);
    }
}