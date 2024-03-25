using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
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

    public FileUpload(IWorkflowDefinitionStore workflowDefinition)
    {
        _workflowDefinition = workflowDefinition;
    }

    [ActivityInput(
        UIHint = ActivityInputUIHints.Dropdown,
        DefaultSyntax = SyntaxNames.Literal,
        Hint = "Mẫu File nhập liệu",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })
    ]
    public string FileTemplate { get; set; } = default!;

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
            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}