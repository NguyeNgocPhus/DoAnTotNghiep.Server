using DoAn.Application.DTOs.Workflow;

namespace DoAn.Application.Abstractions;

public interface IWorkflowExecuteService
{
    Task<bool> ExecuteWorkflowsAsync( ExecuteFileUpdateDto data, CancellationToken cancellationToken = default);
}