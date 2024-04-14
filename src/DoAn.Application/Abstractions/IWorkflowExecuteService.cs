using DoAn.Application.DTOs.Workflow;
using Elsa.Services.Models;

namespace DoAn.Application.Abstractions;

public interface IWorkflowExecuteService
{
    Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync( ExecuteFileUpdateDto data, CancellationToken cancellationToken = default);
}