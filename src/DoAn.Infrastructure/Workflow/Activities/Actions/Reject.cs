using Elsa;
using Elsa.Activities.Signaling.Models;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;

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

    private readonly IWorkflowDefinitionStore _workflowDefinition;

    public Reject(IWorkflowExecutionLogStore workflowExecutionLog, IWorkflowDefinitionStore workflowDefinition)
    {
        _workflowExecutionLog = workflowExecutionLog;
        _workflowDefinition = workflowDefinition;
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
    public string Role { get; set; } = default!;

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

            Output = "REJECT";
            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}