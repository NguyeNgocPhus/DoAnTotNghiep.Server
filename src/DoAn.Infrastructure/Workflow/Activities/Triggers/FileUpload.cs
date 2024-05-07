using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.DTOs.Workflow;
using DoAn.Domain.Entities;
using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;

namespace DoAn.Infrastructure.Workflow.Activities.Triggers;

[Activity(
    DisplayName = "Tải lên",
    Category = "Quy trình phê duyệt",
    Description = "Tải file"
)]
public class FileUpload : Activity
{
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly IRepositoryBase<ActionLogs, Guid> _actionLogsRepository;
    private readonly IRepositoryBase<FileStorage, Guid> _fileRepository;
    private readonly IUnitOfWork _unitOfWork;


    public FileUpload(IWorkflowDefinitionStore workflowDefinition, IRepositoryBase<ActionLogs, Guid> actionLogsRepository, IRepositoryBase<FileStorage, Guid> fileRepository, IUnitOfWork unitOfWork)
    {
        _workflowDefinition = workflowDefinition;
        _actionLogsRepository = actionLogsRepository;
        _fileRepository = fileRepository;
        _unitOfWork = unitOfWork;
    }
    [ActivityInput(
        DefaultSyntax = SyntaxNames.Literal,
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })
    ]
    public string Position { get; set; } = default!;
    [ActivityInput(
        DefaultSyntax = SyntaxNames.Literal,
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })
    ]
    public string Data { get; set; } = default!;
    [ActivityInput(
        DefaultSyntax = SyntaxNames.Literal,
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })
    ]
    public string Description { get; set; } = default!;
    
    

    protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        return context.WorkflowExecutionContext.IsFirstPass ? await OnExecuteInternal(context) : Suspend();
    }

    protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
    {
        return await OnExecuteInternal(context);
    }

    private async Task<IActivityExecutionResult> OnExecuteInternal(ActivityExecutionContext context)
    {
        try
        {
           
            var input = context.GetInput<ExecuteFileUpdateDto>();
            var fileDetail = await _fileRepository.FindSingleAsync(x => x.Id == input.FileId);
            
            // get wf definition by definition id
            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(context.WorkflowInstance.DefinitionId, VersionOptions.Latest, cancellationToken: context.CancellationToken);
            if (workflowDefinition == null)
                throw new Exception("Workflow Definition not found");
            
            context.WorkflowExecutionContext.ContextId = input.FileId.ToString();
            var actionLog = new ActionLogs()
            {
                ActivityId = context.ActivityId,
                ActivityName = nameof(FileUpload),
                CreatedBy = fileDetail.CreatedBy,
                CreatedTime = DateTime.Now,
                ContextId = fileDetail.Id,
                WorkflowInstanceId = context.WorkflowInstance.Id,
                WorkflowDefinitionId = workflowDefinition.Id,
                ActionReason = string.Empty
            };
            _actionLogsRepository.Add(actionLog);
            await _unitOfWork.SaveChangesAsync();
            
            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}