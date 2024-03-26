using DoAn.Application.Abstractions;
using DoAn.Application.DTOs.Workflow;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowExecuteService : IWorkflowExecuteService
{
    public Task<bool> ExecuteWorkflowsAsync(ExecuteFileUpdateDto data, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}