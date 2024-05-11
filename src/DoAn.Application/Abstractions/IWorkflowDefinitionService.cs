using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Queries;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.Abstractions;

public interface IWorkflowDefinitionService
{
    public Task<CreateWorkflowResponse> CreateWorkflowDefinitionAsync(CreateWorkflowDefinitionCommand data, CancellationToken cancellationToken = default);
    public Task<UpdateWorkflowDefinitionResponse> UpdateWorkflowDefinitionAsync(UpdateWorkflowDefinitionCommand data, CancellationToken cancellationToken = default);
    public Task<string> DeleteWorkflowDefinitionAsync(DeleteWorkflowDefinitionCommand data, CancellationToken cancellationToken = default);
    public Task<List<WorkflowDefinitionResponse>> GetListWorkflowDefinitionAsync(GetListWorkflowDefinitionQuery query, CancellationToken cancellationToken = default);
    public Task<WorkflowDefinitionResponse> GetWorkflowDefinitionAsync(string id, CancellationToken cancellationToken = default);
    public Task<object> GetWorkflowActivityAsync(Guid fileId, CancellationToken cancellationToken = default);
}