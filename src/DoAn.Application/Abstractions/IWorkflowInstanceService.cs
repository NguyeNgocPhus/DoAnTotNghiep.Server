using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.Abstractions;

public interface IWorkflowInstanceService
{
    public Task<CurrentStepWorkflowResponse> GetCurrentStepAsync(Guid fileId, CancellationToken cancellationToken = default);
}