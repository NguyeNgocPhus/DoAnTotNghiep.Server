using DoAn.Application.DTOs.Workflow;
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
            var input = context.GetInput<ExecuteFileUpdateDto>();
            
            context.WorkflowExecutionContext.ContextId = input.FileId.ToString();
            
            return Done();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}