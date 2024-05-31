using System.Linq.Expressions;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using Castle.DynamicLinqQueryBuilder;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.DTOs;
using DoAn.Application.Services;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Services.V1.Identity.Responses;
using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DoAn.Infrastructure.Workflow.Activities.Actions;

[Activity(
    DisplayName = "Điều kiện",
    Category = "Quy trình phê duyệt",
    Description = "Nếu người dùng có quền được chỉ định, dữ liệu nhập sẽ được chấp thuận và không cần phê duyệt",
    Outcomes = new[] { OutcomeNames.Done }
)]
public class UpdateStatus : Activity
{
    private readonly IWorkflowExecutionLogStore _workflowExecutionLog;
    private readonly WorkflowExecutionLog _workflowExecution;
    private readonly LinqExpressionService _linqExpressionService;
    private readonly IRepositoryBase<ActionLogs, Guid> _actionLogRepository;
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryBase<ImportTemplate, Guid> _importTemplateRepository;
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStatus(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition,
        ICurrentUserService currentUserService, IRepositoryBase<ActionLogs, Guid> actionLogRepository,
        IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationService notificationService,
        UserManager<AppUser> userManager, IRepositoryBase<ImportHistory, Guid> importHistoryRepository,
        IRepositoryBase<ImportTemplate, Guid> importTemplateRepository, LinqExpressionService linqExpressionService)
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
        _linqExpressionService = linqExpressionService;
    }

    [ActivityOutput] public object? Output { get; set; }

    [ActivityInput(
        Hint = "Quyền người dùng",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal }
    )]
    public string Data { get; set; } = default!;

    protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        try
        {
            var data = JsonConvert.DeserializeObject<JObject>(Data);
            var @object = data["object"]?.ToString();
            var status = data["status"]?.ToString();
            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                cancellationToken: context.CancellationToken);
            if (@object == "ImportHistory")
            {
                var importHistory =
                    await _importHistoryRepository.FindSingleAsync(x => x.FileId == Guid.Parse(context.ContextId));
                importHistory.Status = status.Trim().ToUpper();
                await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            }
            
            var actionLog_n = new ActionLogs()
            {
                ActivityId = context.ActivityId,
                ActivityName = nameof(UpdateStatus),
                CreatedBy = Guid.Empty,
                CreatedTime = DateTime.Now,
                ContextId = Guid.Parse(context.ContextId),
                WorkflowInstanceId = context.WorkflowInstance.Id,
                WorkflowDefinitionId = workflowDefinition.Id,
                ActionReason = string.Empty,
                Data = ""
            };
            
            _actionLogRepository.Add(actionLog_n);

            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}