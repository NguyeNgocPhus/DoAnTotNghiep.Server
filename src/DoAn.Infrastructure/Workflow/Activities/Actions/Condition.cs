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

namespace DoAn.Infrastructure.Workflow.Activities.Actions;

[Activity(
    DisplayName = "Điều kiện",
    Category = "Quy trình phê duyệt",
    Description = "Nếu người dùng có quền được chỉ định, dữ liệu nhập sẽ được chấp thuận và không cần phê duyệt",
    Outcomes = new[] { True, False }
)]
public class Condition : Activity
{
    public const string True = "True";
    public const string False = "False";
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
    
    public Condition(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition,
        ICurrentUserService currentUserService, IRepositoryBase<ActionLogs, Guid> actionLogRepository,
        IUnitOfWork unitOfWork, IUserRepository userRepository, INotificationService notificationService, UserManager<AppUser> userManager, IRepositoryBase<ImportHistory, Guid> importHistoryRepository, IRepositoryBase<ImportTemplate, Guid> importTemplateRepository, LinqExpressionService linqExpressionService)
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
            Data = Data.Replace("combinator", "condition");
            Data = Data.Replace("=", "in");
            Data = Data.Replace("!=", "not_equal");
            
            
            var actionLog =
                await _actionLogRepository.FindSingleAsync(x => x.ContextId == Guid.Parse(context.ContextId));
            var user = await _userRepository.GetUserByIdAsync(actionLog.CreatedBy);

            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                cancellationToken: context.CancellationToken);
            var importHistory =
               await _importHistoryRepository.FindSingleAsync(x => x.FileId == Guid.Parse(context.ContextId));
            
            var filter = JsonConvert.DeserializeObject<QueryRule>(Data);

            var objectFilter = new ObjectFilter()
            {
                Roles = user.Roles.ToList(),
                CreatedTime = importHistory.CreatedTime
            };
            // linq dynamic expression
            var expression = _linqExpressionService.ParseExpressionOf<ObjectFilter>(filter);
            
            var query = new List<ObjectFilter>()
            {
                objectFilter
            }.AsQueryable();
            var checkValid = query.Where(expression).ToList();
            
            var actionLog_n = new ActionLogs()
            {
                ActivityId = context.ActivityId,
                ActivityName = nameof(Condition),
                CreatedBy = Guid.Empty,
                CreatedTime = DateTime.Now,
                ContextId = Guid.Parse(context.ContextId),
                WorkflowInstanceId = context.WorkflowInstance.Id,
                WorkflowDefinitionId = workflowDefinition.Id,
                ActionReason = string.Empty
            };
            
          
            
            if (checkValid.Any())
            {
                Output = "TRUE";
                actionLog_n.Data = True;
                _actionLogRepository.Add(actionLog_n);

                await _unitOfWork.SaveChangesAsync(context.CancellationToken);
                return Outcome(True);
            }
            actionLog_n.Data = False;
            _actionLogRepository.Add(actionLog_n);
            await _unitOfWork.SaveChangesAsync(context.CancellationToken);
            return Outcome(False);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    class ObjectFilter
    {
        public List<string> Roles { get; set; }
        public DateTime CreatedTime { get; set; }
    }
    
   
}