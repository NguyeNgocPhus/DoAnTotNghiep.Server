using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Infrastructure.Workflow.Specifications;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Elsa;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Microsoft.AspNetCore.Http;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowInstanceService : IWorkflowInstanceService
{
    private readonly IWorkflowLaunchpad _workflowLaunchpad;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IWorkflowInstanceStore _workflowInstance;
    private readonly IWorkflowDefinitionStore _workflowDefinition;

    public WorkflowInstanceService(IWorkflowLaunchpad workflowLaunchpad, IHttpContextAccessor httpContext,
        IWorkflowInstanceStore workflowInstance, IWorkflowDefinitionStore workflowDefinition)
    {
        _workflowLaunchpad = workflowLaunchpad;
        _httpContext = httpContext;
        _workflowInstance = workflowInstance;
        _workflowDefinition = workflowDefinition;
    }

    public async Task<CurrentStepWorkflowResponse> GetCurrentStepAsync(Guid fileId,
        CancellationToken cancellationToken = default)
    {
        var workflowInstance =
            await _workflowInstance.FindAsync(new WorkflowInstanceContextIdSpecification(fileId.ToString()),
                cancellationToken);

        if (workflowInstance == null)
            throw new WorkflowInstanceNotFoundException("Workflow instance not found");
        var blockActivity = workflowInstance.BlockingActivities;

        // get wf definition by definition id
        var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(workflowInstance.DefinitionId,
            VersionOptions.Latest, cancellationToken: cancellationToken);

        var activitiesDefinition =
            workflowDefinition!.Activities.Where(x => x.Properties.Any(xx => xx.Name == "Signal"));

        // current activities are blocking
        var currentBlockingActivities = activitiesDefinition
            .Where(x => blockActivity.Select(r => r.ActivityId).Contains(x.ActivityId)).ToList();


        return new CurrentStepWorkflowResponse()
        {
            WorkflowInstanceId = workflowInstance.Id,
            Activities = currentBlockingActivities.Select(x => new CurrentActivity()
            {
                ActivityId = x.ActivityId,
                Signal = x.Properties.First(p => p.Name == "Signal").Expressions.First(x => x.Key == "Literal").Value
            }).ToList()
        };
    }
}