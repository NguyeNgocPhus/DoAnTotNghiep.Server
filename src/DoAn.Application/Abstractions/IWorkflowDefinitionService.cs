using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.Abstractions;

public interface IWorkflowDefinitionService
{
    public Task<CreateWorkflowResponse> CreateWorkflowAsync(CreateWorkflowCommand data);
}