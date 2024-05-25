using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
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

namespace DoAn.Infrastructure.Workflow.Activities.Actions
{
    [Activity(
        DisplayName = "Kết thúc",
        Category = "Quy trình phê duyệt",
        Description = "Kết thúc quy trình",
        Outcomes = new string[0]
    )]
    public class Finish : Activity
    {
        private readonly IWorkflowDefinitionStore _workflowDefinition;
        private readonly IRepositoryBase<ImportHistory, Guid> _importHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryBase<ActionLogs, Guid> _actionLogRepository;
        public Finish(IWorkflowDefinitionStore workflowDefinition,
            IRepositoryBase<ImportHistory, Guid> importHistoryRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IRepositoryBase<ActionLogs, Guid> actionLogRepository)
        {
            _workflowDefinition = workflowDefinition;
            _importHistoryRepository = importHistoryRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _actionLogRepository = actionLogRepository;
        }

        [ActivityInput(Hint = "The value to set as the workflow's output",
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public object? ActivityOutput { get; set; }

        [ActivityOutput] public object? Output { get; set; }
        [ActivityInput] public object Input { get; set; }

        [ActivityInput(
            Hint = "The outcomes to set on the container activity",
            UIHint = ActivityInputUIHints.MultiText,
            DefaultValue = new[] { Elsa.OutcomeNames.Done },
            DefaultSyntax = SyntaxNames.Json,
            SupportedSyntaxes = new[] { SyntaxNames.Json },
            IsDesignerCritical = true)]
        public ICollection<string> OutcomeNames { get; set; } = new[] { Elsa.OutcomeNames.Done };

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            try
            {
                var parentBlueprint = context.ActivityBlueprint.Parent;
                var isRoot = parentBlueprint == null;

                // Remove any blocking activities within the scope of the composite activity.
                await context.WorkflowExecutionContext.RemoveBlockingActivitiesAsync(parentBlueprint?.Id);

                // Evict & remove any scope activities within the scope of the composite activity.
                var scopes = context.WorkflowInstance.Scopes.Select(x => x).Reverse().ToList();
                var scopeIds = scopes.Select(x => x.ActivityId).ToList();
                var containedScopeActivityIds = parentBlueprint == null
                    ? scopeIds
                    : parentBlueprint.Activities.Where(x => scopeIds.Contains(x.Id)).Select(x => x.Id).ToList();

                foreach (var scopeId in containedScopeActivityIds)
                {
                    var scopeActivity = context.WorkflowExecutionContext.GetActivityBlueprintById(scopeId)!;
                    await context.WorkflowExecutionContext.EvictScopeAsync(scopeActivity);
                    scopes.RemoveAll(x => x.ActivityId == scopeId);
                }

                context.WorkflowInstance.Scopes = new SimpleStack<ActivityScope>(scopes.AsEnumerable().Reverse());

                // Return output.
                Output = new FinishOutput(ActivityOutput, OutcomeNames);
                context.LogOutputProperty(this, nameof(Output), Output);

                if (isRoot)
                {
                    // Clear activity scheduler to prevent other scheduled activities from adding new blocking activities, which would prevent the workflow from completing.
                    context.WorkflowExecutionContext.ClearScheduledActivities();
                }
                else
                {
                    // Clear all activities scheduled by the parent composite.
                    context.WorkflowExecutionContext.ClearScheduledActivities(parentBlueprint!.Id);
                }
                var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(
                    context.WorkflowInstance.DefinitionId, VersionOptions.Latest,
                    cancellationToken: context.CancellationToken);
                if (workflowDefinition == null)
                    throw new Exception("Approve activity: Workflow Definition not found");
                var userId = _currentUserService.UserId;
                var importHistory =
                    await _importHistoryRepository.FindSingleAsync(x => x.FileId == Guid.Parse(context.ContextId));

                if (importHistory == null)
                    throw new Exception("Import hisotry is not found");

                importHistory.Status = context.Input switch
                {
                    "TRUE" => "APPROVE",
                    "APPROVE" => "APPROVE",
                    "REJECT" => "REJECT",
                    _ => importHistory.Status
                };
                var actionLog = new ActionLogs()
                {
                    ActivityId = context.ActivityId,
                    ActivityName = nameof(Finish),
                    CreatedBy = Guid.Parse(userId),
                    CreatedTime = DateTime.Now,
                    ContextId = Guid.Parse(context.ContextId),
                    WorkflowInstanceId = context.WorkflowInstance.Id,
                    WorkflowDefinitionId = workflowDefinition.Id,
                    ActionReason = string.Empty
                };
                _actionLogRepository.Add(actionLog);
                await _unitOfWork.SaveChangesAsync(context.CancellationToken);

                return Noop();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}