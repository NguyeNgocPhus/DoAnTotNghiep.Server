using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;

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


    [ActivityOutput] public object? Output { get; set; }
    //[ActivityInput(Hint = "The condition to evaluate.", UIHint = ActivityInputUIHints.SingleLine, SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
    //public bool Condition { get; set; }


    [ActivityInput(
        UIHint = ActivityInputUIHints.CheckList,
        Hint = "Quyền người dùng",
        DefaultSyntax = SyntaxNames.Json,
        SupportedSyntaxes = new[] { SyntaxNames.Json }
    )]
    public ISet<string> Roles { get; set; } = new HashSet<string>();

    protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
    {
        try
        {
            if (Roles.Any())
            {
                Output = "APPROVE";
                return Outcome(True);
            }
            else
            {
                return Outcome(False);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}