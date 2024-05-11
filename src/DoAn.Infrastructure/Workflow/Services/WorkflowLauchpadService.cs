using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.DTOs.Workflow;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities;
using DoAn.Infrastructure.Workflow.Activities.Triggers;
using DoAn.Infrastructure.Workflow.Bookmarks;
using DoAn.Shared.Services.V1.Workflow.Commands;
using Elsa;
using Elsa.Activities.Signaling.Models;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Open.Linq.AsyncExtensions;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowLaunchpadService : IWorkflowLaunchpadService
{
    private readonly IWorkflowLaunchpad _workflowLaunchpad;
    private readonly IWorkflowInstanceStore _workflowInstance;
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly IRepositoryBase<FileStorage, Guid> _repository;
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistory;
    public WorkflowLaunchpadService(IWorkflowLaunchpad workflowLaunchpad, IWorkflowInstanceStore workflowInstance, IWorkflowDefinitionStore workflowDefinition, IRepositoryBase<FileStorage, Guid> repository, IRepositoryBase<ImportHistory, Guid> importHistory)
    {
        _workflowLaunchpad = workflowLaunchpad;
        _workflowInstance = workflowInstance;
        _workflowDefinition = workflowDefinition;
        _repository = repository;
        _importHistory = importHistory;
    }

    public async Task<IEnumerable<CollectedWorkflow>> StartWorkflowsAsync(ExecuteFileUpdateDto data,
        CancellationToken cancellationToken = default)
    {
        var query =
            new
            {
                importTemplateId = data.ImportTemplateId
            };
        var filteredContext = new WorkflowsQuery(ActivityType: nameof(FileUpload),
            Bookmark: new FileUploadBookmark(JsonConvert.SerializeObject(query)));
        var filteredWorkflows =
            await _workflowLaunchpad.FindWorkflowsAsync(filteredContext, cancellationToken).ToList();

        await _workflowLaunchpad.ExecutePendingWorkflowsAsync(filteredWorkflows, new WorkflowInput(data),
            cancellationToken);

        return filteredWorkflows;
    }

    public async Task<object> ResumeWorkflowsAsync(ExecuteWorkflowCommand data, CancellationToken cancellationToken = default)
    {
        var workflowInstance = await _workflowInstance.FindByIdAsync(data.WorkflowInstanceId, cancellationToken) ??
                               throw new WorkflowInstanceNotFoundException("wf instance is null");
        
        if (workflowInstance.BlockingActivities.FirstOrDefault(x => x.ActivityId == data.ActivityId) == null)
        {
            throw new Exception("Không thể phê duyệt");
        }
        // //kiểm tra current user có được phân quyền dữ liệu với đơn vị hay không.
        var importHistory =
            await _importHistory.FindAll(x => x.FileId == Guid.Parse(workflowInstance.ContextId)).FirstOrDefaultAsync(cancellationToken);
        
        var activityShouldBeDelete =
            workflowInstance.BlockingActivities.Where(x => x.ActivityId != data.ActivityId).ToList();
        
        var result = await _workflowLaunchpad.ExecutePendingWorkflowAsync(data.WorkflowInstanceId,
            data.ActivityId, new WorkflowInput(new Signal(data.Signal, data.RejectReason)), cancellationToken);

        //nếu phê duyệt thì không được từ chối và ngược lại
        if (result.Executed)
        {
            var newWorkflowInstance =
                await _workflowInstance.FindByIdAsync(data.WorkflowInstanceId, cancellationToken) ??
                throw new Exception("wf instance is null");
            newWorkflowInstance.BlockingActivities.RemoveWhere(x =>
                activityShouldBeDelete.Select(xx => xx.ActivityId).Contains(x.ActivityId));
            await _workflowInstance.UpdateAsync(newWorkflowInstance, cancellationToken);

            await _workflowInstance.SaveAsync(newWorkflowInstance, cancellationToken);
        }
        
        return data;
    }
}