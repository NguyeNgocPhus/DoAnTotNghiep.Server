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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rebus.Pipeline;

namespace DoAn.Infrastructure.Workflow.Activities.Actions;

[Trigger(
    DisplayName = "Phê duyệt",
    Category = "Quy trình phê duyệt",
    Description = "Chờ phê duyệt",
    Outcomes = new[] { OutcomeNames.Done }
)]
public class Approve : Activity
{
    private readonly IWorkflowExecutionLogStore _workflowExecutionLog;
    private readonly WorkflowExecutionLog _workflowExecution;
    private readonly IRepositoryBase<ActionLogs, Guid> _actionLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
    private readonly IRepositoryBase<ImportTemplate, Guid> _importTemplateRepository;
    
    public Approve(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition,
        ICurrentUserService currentUserService, IRepositoryBase<ActionLogs, Guid> actionLogRepository,
        IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationService notificationService, UserManager<AppUser> userManager, IRepositoryBase<ImportHistory, Guid> importHistoryRepository, IRepositoryBase<ImportTemplate, Guid> importTemplateRepository)
    {
        _workflowExecutionLog = workflowExecutionLog;

        _workflowDefinition = workflowDefinition;
        _currentUserService = currentUserService;
        _actionLogRepository = actionLogRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _userManager = userManager;
        _importHistoryRepository = importHistoryRepository;
        _importTemplateRepository = importTemplateRepository;
    }

    [ActivityOutput] public object? Output { get; set; }

    [ActivityInput(Hint = "The name of the signal to wait for.",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    public string Signal { get; set; } = default!;

    [ActivityInput(
        Hint = "Quyền người dùng",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid }
    )]
    public string Data { get; set; } = default!;

    [ActivityInput(
        DefaultSyntax = SyntaxNames.Literal,
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal })
    ]
    public string Position { get; set; } = default!;


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
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId);
            var importHistory = await _importHistoryRepository.FindSingleAsync(x => x.FileId == Guid.Parse(context.ContextId));
            var importTemplate =
                await _importTemplateRepository.FindSingleAsync(x => x.Id == importHistory.ImportTemplateId);

            
            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                cancellationToken: context.CancellationToken);
            if (workflowDefinition == null)
                throw new Exception("Approve activity: Workflow Definition not found");
            var actionLog = new ActionLogs()
            {
                ActivityId = context.ActivityId,
                ActivityName = nameof(Approve),
                CreatedBy = Guid.Parse(userId),
                CreatedTime = DateTime.Now,
                ContextId = Guid.Parse(context.ContextId),
                WorkflowInstanceId = context.WorkflowInstance.Id,
                WorkflowDefinitionId = workflowDefinition.Id,
                ActionReason = string.Empty
            };
            ///////////////////// GỬI EMAIL ĐẾN CẤP 2////////////////////////////////
            // get to next activity
            var nextActivities = new List<ActivityDefinition>();
            GetNextActivities(context.ActivityId);

            void GetNextActivities(string activityId)
            {
                if (nextActivities.Count > 0) return;
                foreach (var connection in workflowDefinition.Connections)
                {
                    if (connection.SourceActivityId == activityId)
                    {
                        var activity =
                            workflowDefinition.Activities.FirstOrDefault(x =>
                                x.ActivityId == connection.TargetActivityId);
                        if (activity.Type == nameof(Approve) || activity.Type == nameof(Reject))
                        {
                            nextActivities.Add(activity);
                        }

                        GetNextActivities(connection.TargetActivityId);
                    }
                }
            }

            // get role of next activity
            var roles = nextActivities.Select(x => x.Properties.FirstOrDefault(xx => xx.Name == "Data").Expressions.FirstOrDefault().Value.Trim()).Distinct();
            if (roles.Any())
            {
                var value = JsonConvert.DeserializeObject<JObject>(roles.First());
                var roleId = value?["roleId"].ToObject<Guid>()?? null;
                if (roleId != null)
                {
                    var users = await _userRepository.GetUserHasRole(roleId.Value, context.CancellationToken);

                    var fields = new Dictionary<string, string>()
                    {
                        {"UserName", user.UserName},
                        {"ImportTemplateName",importTemplate.Name},
                        {"Code",importHistory.Code}
                    };
                    await _notificationService.SendNotificationAsync(users.Select(x => x.Id).ToList(), NotificationType.Approve, fields, importHistory.Id.ToString());
                }
            }
            ///////////////////// GỬI EMAIL ĐẾN NGƯỜI UPLOAD////////////////////////////////
            var actionLogs = await _actionLogRepository.AsQueryable()
                .Where(x => x.ContextId == Guid.Parse(context.ContextId))
                .ToListAsync();
            if (actionLogs.Any())
            {
                var fields = new Dictionary<string, string>()
                {
                    { "UserName", user.UserName },
                    { "ImportTemplateName", importTemplate.Name },
                    { "Code", importHistory.Code },
                };
                await _notificationService.SendNotificationAsync(actionLogs.Select(x => x.CreatedBy).ToList(),
                    NotificationType.Approve, fields, importHistory.Id.ToString());

            }
            

            // add action log
            _actionLogRepository.Add(actionLog);

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);

            Output = "APPROVE";

            return Done();
        }
        catch (Exception)
        {
            throw;
        }
    }
}