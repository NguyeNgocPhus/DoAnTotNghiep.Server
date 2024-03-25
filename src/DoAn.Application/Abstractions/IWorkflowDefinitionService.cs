using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.Abstractions;

public interface IWorkflowDefinitionService
{
    public Task<CreateWorkflowResponse> CreateWorkflowDefinitionAsync(CreateWorkflowDefinitionCommand data, CancellationToken cancellationToken = default);
    public Task<UpdateWorkflowDefinitionResponse> UpdateWorkflowDefinitionAsync(UpdateWorkflowDefinitionCommand data, CancellationToken cancellationToken = default);
    
    public Task<List<WorkflowDefinitionResponse>> GetListWorkflowDefinitionAsync(CancellationToken cancellationToken = default);
}