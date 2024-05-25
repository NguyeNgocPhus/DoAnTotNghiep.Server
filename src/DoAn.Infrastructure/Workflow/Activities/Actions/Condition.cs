using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using Castle.DynamicLinqQueryBuilder;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using DoAn.Shared.Services.V1.Identity.Responses;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
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
    private readonly IRepositoryBase<ActionLogs, Guid> _actionLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotificationService _notificationService;
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
    private readonly IRepositoryBase<ImportTemplate, Guid> _importTemplateRepository;
    
    public Condition(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition,
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

            var filter = JsonConvert.DeserializeObject<QueryRule>(Data);
            var role = filter.Rules.FirstOrDefault();
            if (role != null)
            { 
                if (user.Roles.Contains(role.Value.ToString()))
                {
                    Output = "TRUE";
                    return Outcome(True);
                }
                return Outcome(False);
                
            }
            return Outcome(False);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
    public class QueryRule : IFilterRule
    {
        public string Condition { get; set; }
        public string Field { get; set; }
        public string Id { get; set; }
        public string Input { get; set; }
        public string Operator { get; set; }
        public IEnumerable<QueryRule> Rules { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public string Valid { get; set; }
        IEnumerable<IFilterRule> IFilterRule.Rules => Rules;
        object IFilterRule.Value => Value;
    }
}