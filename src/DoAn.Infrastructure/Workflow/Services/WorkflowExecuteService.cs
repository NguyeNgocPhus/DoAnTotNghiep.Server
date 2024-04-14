using DoAn.Application.Abstractions;
using DoAn.Application.DTOs.Workflow;
using DoAn.Infrastructure.Workflow.Activities.Triggers;
using DoAn.Infrastructure.Workflow.Bookmarks;
using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;
using Open.Linq.AsyncExtensions;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowExecuteService : IWorkflowExecuteService
{
    private readonly IWorkflowLaunchpad _workflowLaunchpad;

    public WorkflowExecuteService(IWorkflowLaunchpad workflowLaunchpad)
    {
        _workflowLaunchpad = workflowLaunchpad;
    }

    public async Task<IEnumerable<CollectedWorkflow>> ExecuteWorkflowsAsync(ExecuteFileUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var query =
            new
            {
                importTemplateId = data.ImportTemplateId
            };
        var filteredContext = new WorkflowsQuery(ActivityType: nameof(FileUpload),
            Bookmark: new FileUploadBookmark(Newtonsoft.Json.JsonConvert.SerializeObject(query)));
        var filteredWorkflows =
            await _workflowLaunchpad.FindWorkflowsAsync(filteredContext, cancellationToken).ToList();

        await _workflowLaunchpad.ExecutePendingWorkflowsAsync(filteredWorkflows, new WorkflowInput(data),
            cancellationToken);

        return filteredWorkflows;
    }
}