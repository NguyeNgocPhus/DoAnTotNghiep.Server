using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using Elsa;
using Elsa.Activities.Signaling.Models;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Esprima.Ast;

namespace DoAn.Infrastructure.Workflow.Activities.Actions;

[Trigger(
    DisplayName = "Từ chối",
    Category = "Quy trình phê duyệt",
    Description = "Chờ từ chối",
    Outcomes = new[] { OutcomeNames.Done }
)]
public class Reject : Activity
{
    private readonly IWorkflowExecutionLogStore _workflowExecutionLog;
    private readonly WorkflowExecutionLog _workflowExecution;
    private readonly IRepositoryBase<ActionLogs, Guid> _actionLogRepository;
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly ICurrentUserService _currentUserService;

    public Reject(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition,
        IRepositoryBase<ActionLogs, Guid> actionLogRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService, IRepositoryBase<ImportHistory, Guid> importHistoryRepository)
    {
        _workflowExecutionLog = workflowExecutionLog;
        _workflowDefinition = workflowDefinition;
        _actionLogRepository = actionLogRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _importHistoryRepository = importHistoryRepository;
    }

    [ActivityInput(
        Hint = "The name of the signal to wait for.",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
    )]
    public string Signal { get; set; } = default!;

    [ActivityInput(
        UIHint = ActivityInputUIHints.Dropdown,
        Hint = "Quyền người dùng",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
    )]
    public string Data { get; set; } = default!;


    [ActivityInput(
        DefaultSyntax = SyntaxNames.Literal,
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })
    ]
    public string Position { get; set; } = default!;

    [ActivityInput(
        DefaultSyntax = SyntaxNames.Literal,
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })
    ]
    public string Description { get; set; } = default!;


    [ActivityOutput] public object? Output { get; set; }


    protected override bool OnCanExecute(ActivityExecutionContext context)
    {
        if (context.Input is Signal triggeredSignal)
            return string.Equals(triggeredSignal.SignalName, Signal, StringComparison.OrdinalIgnoreCase);

        return false;
    }

    protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context) =>
        context.WorkflowExecutionContext.IsFirstPass ? await OnResumeAsync(context) : Suspend();

    protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
    {
        try
        {
            var triggeredSignal = context.GetInput<Signal>()!;
            var rejectReason = triggeredSignal.Input?.ToString() ?? string.Empty;
          
            var userId = _currentUserService.UserId;
            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                cancellationToken: context.CancellationToken);
            if (workflowDefinition == null)
                throw new Exception("Approve activity: Workflow Definition not found");

         
            // importHistory.Status = "REJECT";
            
            var actionLog = new ActionLogs()
            {
                ActivityId = context.ActivityId,
                ActivityName = nameof(Reject),
                CreatedBy = Guid.Parse(userId),
                CreatedTime = DateTime.Now,
                ContextId = Guid.Parse(context.ContextId),
                WorkflowInstanceId = context.WorkflowInstance.Id,
                WorkflowDefinitionId = workflowDefinition.Id,
                ActionReason = rejectReason
            };

            // add action log
            _actionLogRepository.Add(actionLog);

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            
            Output = "REJECT";
            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}