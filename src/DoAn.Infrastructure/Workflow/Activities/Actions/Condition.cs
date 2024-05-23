using System.Runtime.InteropServices.JavaScript;
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
    
    [ActivityInput(
        Hint = "Quyền người dùng",
        SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Literal }
    )]
    public string Data { get; set; } = default!;

    protected override IActivityExecutionResult OnExecute(ActivityExecutionContext context)
    {
        try
        {
            var d = Data;
            Output = "APPROVE";
            return Outcome(True);
        }
        catch (Exception)
        {
            throw;
        }
    }
}