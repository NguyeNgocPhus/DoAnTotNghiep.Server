namespace DoAn.Shared.Services.V1.Workflow.Common;

public class ActivityProperty
{
    public string Name { get; set; } = default!;

    /// <summary>
    /// The configured syntax to use when selecting the expression to evaluate.
    /// </summary>
    /// <remarks>
    /// If no value is specified (i.e. null or empty), then an attempt will be made to select for the "Literal" syntax expression.
    /// </remarks>
    public string? Syntax { get; set; }

    /// <summary>
    /// Contains an expression for each supported syntax.
    /// </summary>
    public IDictionary<string, string?> Expressions { get; set; } = new Dictionary<string, string?>();

    public string? GetExpression(string syntax) => Expressions.ContainsKey(syntax) ? Expressions[syntax] : default;
}