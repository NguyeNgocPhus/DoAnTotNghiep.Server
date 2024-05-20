using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryBase<ImportTemplate, Guid> _importTemplateRepository;

    public Reject(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition,
        IRepositoryBase<ActionLogs, Guid> actionLogRepository, IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService, IRepositoryBase<ImportHistory, Guid> importHistoryRepository,
        IUserRepository userRepository, UserManager<AppUser> userManager, INotificationService notificationService,
        IRepositoryBase<ImportTemplate, Guid> importTemplateRepository)
    {
        _workflowExecutionLog = workflowExecutionLog;
        _workflowDefinition = workflowDefinition;
        _actionLogRepository = actionLogRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _importHistoryRepository = importHistoryRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _notificationService = notificationService;
        _importTemplateRepository = importTemplateRepository;
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

            var user = await _userManager.FindByIdAsync(userId);
            var importHistory =
                await _importHistoryRepository.FindSingleAsync(x => x.FileId == Guid.Parse(context.ContextId));
            var importTemplate =
                await _importTemplateRepository.FindSingleAsync(x => x.Id == importHistory.ImportTemplateId);


            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                cancellationToken: context.CancellationToken);
            if (workflowDefinition == null)
                throw new Exception("Approve activity: Workflow Definition not found");

            ///////////////////// GỬI EMAIL ĐẾN NGƯỜI UPLOAD////////////////////////////////
            var actionLogs = await _actionLogRepository.AsQueryable()
                .Where(x => x.ContextId == Guid.Parse(context.ContextId))
                .ToListAsync();


            var fields = new Dictionary<string, string>()
            {
                { "UserName", user.UserName },
                { "ImportTemplateName", importTemplate.Name }, { "Code", importHistory.Code }
            };
            await _notificationService.SendNotificationAsync(actionLogs.Select(x => x.CreatedBy).ToList(),
                NotificationType.Reject, fields, importHistory.Id.ToString());


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