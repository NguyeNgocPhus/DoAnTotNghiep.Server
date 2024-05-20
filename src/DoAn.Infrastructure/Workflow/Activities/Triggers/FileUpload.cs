using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.DTOs.Workflow;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using DoAn.Infrastructure.Workflow.Activities.Actions;
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
    private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
    private readonly IRepositoryBase<ImportTemplate, Guid> _importTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<AppUser> _userManager;


    public FileUpload(IWorkflowDefinitionStore workflowDefinition,
        IRepositoryBase<ActionLogs, Guid> actionLogsRepository, IRepositoryBase<FileStorage, Guid> fileRepository,
        IUnitOfWork unitOfWork, INotificationService notificationService, IUserRepository userRepository,
        ICurrentUserService currentUserService, UserManager<AppUser> userManager,
        IRepositoryBase<ImportHistory, Guid> importHistoryRepository,
        IRepositoryBase<ImportTemplate, Guid> importTemplateRepository)
    {
        _workflowDefinition = workflowDefinition;
        _actionLogsRepository = actionLogsRepository;
        _fileRepository = fileRepository;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _importHistoryRepository = importHistoryRepository;
        _importTemplateRepository = importTemplateRepository;
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
            var userId = _currentUserService.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            var input = context.GetInput<ExecuteFileUpdateDto>();
            var fileDetail = await _fileRepository.FindSingleAsync(x => x.Id == input.FileId);

            var importHistory = await _importHistoryRepository.FindSingleAsync(x => x.FileId == input.FileId);
            var importTemplate =
                await _importTemplateRepository.FindSingleAsync(x => x.Id == importHistory.ImportTemplateId);

            // get wf definition by definition id
            var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                cancellationToken: context.CancellationToken);
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

            ///////////////////// Send notification to next step////////////////////////////////
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
            var roles = nextActivities.Select(x =>
                    x.Properties.FirstOrDefault(xx => xx.Name == "Data").Expressions.FirstOrDefault().Value.Trim())
                .Distinct();

            var value = JsonConvert.DeserializeObject<JObject>(roles.First());
            var roleId = value?["roleId"].ToObject<Guid>() ?? null;
            if (roleId != null)
            {
                var users = await _userRepository.GetUserHasRole(roleId.Value, context.CancellationToken);

                var fields = new Dictionary<string, string>()
                {
                    { "UserName", user.UserName },
                    { "ImportTemplateName", importTemplate.Name },
                    { "Code", importHistory.Code }
                };
                await _notificationService.SendNotificationAsync(users.Select(x => x.Id).ToList(),
                    NotificationType.Upload, fields, importHistory.Id.ToString());
            }

            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}