using DoAn.Application.Abstractions;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Elsa.Models;
using Elsa.Persistence;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowDefinitionService : IWorkflowDefinitionService
{
    private readonly IWorkflowDefinitionStore _workflowDefinitionStore;

    public WorkflowDefinitionService(IWorkflowDefinitionStore workflowDefinitionStore)
    {
        _workflowDefinitionStore = workflowDefinitionStore;
    }

    public async Task<CreateWorkflowResponse> CreateWorkflowAsync(CreateWorkflowCommand data)
    {
        var workflowDefinition = new WorkflowDefinition()
        {
            Name = data.Name,
            DisplayName = data.Name,
            Description = data.Description,
            DefinitionId = Guid.NewGuid().ToString(),
            IsLatest = true, 
            PersistenceBehavior = WorkflowPersistenceBehavior.WorkflowBurst
        };
        await _workflowDefinitionStore.SaveAsync(workflowDefinition);

        return new CreateWorkflowResponse()
        {
            DefinitionId = workflowDefinition.DefinitionId,
            Name = data.Name,
            Description = data.Description
        };
    }
}