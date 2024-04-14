using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

namespace DoAn.Infrastructure.Workflow.Activities.Actions;

[Activity(
    DisplayName = "Luồng rẽ nhánh",
    Category = "Quy trình phê duyệt",
    Description = "Thiết lập luồng phê duyệt",
    Outcomes = new string[0])]
public class Branch : Activity
{
    [ActivityInput(
        Hint = "Enter one or more branch names.",
        UIHint = ActivityInputUIHints.MultiText,
        DefaultSyntax = SyntaxNames.Json,
        SupportedSyntaxes = new[] { SyntaxNames.Json },
        IsDesignerCritical = true,
        ConsiderValuesAsOutcomes = true
    )]
    public ISet<string> Branches { get; set; } = new HashSet<string>();
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


    // Schedule the branches in reverse order so that the first branch will execute before the second one, etc.
    // This is important for scenarios where the user needs to schedule a blocking activity like a signal received event before actually sending a signal from a second second branch, causing a response signal to be triggered from another workflow (as an example).
    protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context) => Outcomes(Branches.Reverse());
}