using Elsa;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Elsa.Services.Models;

namespace DoAn.Infrastructure.Workflow.Activities.Actions;
[Activity(
    DisplayName = "Gửi Email",
    Category = "Quy trình phê duyệt",
    Description = "Gửi Email"
)]
public class SendEmail : Activity
{
      
    private readonly IWorkflowDefinitionStore _workflowDefinition;
       
    private readonly IWorkflowInstanceStore _workflowInstance;
    public SendEmail( IWorkflowDefinitionStore workflowDefinition,  IWorkflowInstanceStore workflowInstance)
    {
           
        _workflowDefinition = workflowDefinition;
           
        _workflowInstance = workflowInstance;
    }
    [ActivityOutput]
    public object? Output { get; set; }
    
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

    protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
    {
        // get wf definition by definition id
        var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(context.WorkflowInstance.DefinitionId, VersionOptions.Latest, cancellationToken: context.CancellationToken);
            

        Output = context.Input;

        return Done();
    }
}