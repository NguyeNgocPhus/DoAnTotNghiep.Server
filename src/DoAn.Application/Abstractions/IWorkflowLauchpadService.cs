using DoAn.Application.DTOs.Workflow;
using DoAn.Shared.Services.V1.Workflow.Commands;
using Elsa.Services.Models;

namespace DoAn.Application.Abstractions;

public interface IWorkflowLaunchpadService
{
    Task<IEnumerable<CollectedWorkflow>> StartWorkflowsAsync( ExecuteFileUpdateDto data, CancellationToken cancellationToken = default);
    Task<bool> ResumeWorkflowsAsync( ExecuteWorkflowCommand data, CancellationToken cancellationToken = default);
}