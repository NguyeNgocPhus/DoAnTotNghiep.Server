using System.Linq.Expressions;
using System.Reflection;
using DoAn.Application.Abstractions;
using DoAn.Application.DTOs;

namespace DoAn.Application.Services;

public class LinqExpressionService 
{
    private readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(
            BindingFlags.Static | BindingFlags.Public)
        .Single(m => m.Name == nameof(Enumerable.Contains)
                     && m.GetParameters().Length == 2);
    private Expression Bind(Expression? left, Expression right, string condition)
    {
        if (left == null)
        {
            left = right;
        }
        else
        {
            if (condition == "and")
                left = Expression.And(left, right);
            if (condition == "or")
                left = Expression.Or(left, right);
        }

        return left;
    }


    public Expression ParseTree<T>(QueryRule root, ParameterExpression parm)
    {
        Expression left = null;
        
        foreach (var rule in root.Rules)
        {

            if (rule.Condition is "or" or "and")
            {
                var right = ParseTree<T>(rule, parm);
                left = Bind(left, right, rule.Condition);
                continue;
            }
            if (rule.Operator == "contains")
            {
                var contains = MethodContains.MakeGenericMethod(typeof(string));
                var val = rule.Value.ToString();
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.Call(
                    contains,
                    property,
                    Expression.Constant(val));
                left = Bind(left, right, rule.Condition);
            }
            
            if (rule.Operator == "=")
            {
                var contains = MethodContains.MakeGenericMethod(typeof(string));
                var val = rule.Value.ToString();
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.Call(
                    contains,
                    property,
                    Expression.Constant(val));
                left = Bind(left, right, rule.Condition);
            }

            if (rule.Operator == "!=")
            {
                var val = rule.Value.ToString();
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.NotEqual(
                    property,
                    Expression.Constant(val));
                left = Bind(left, right, rule.Condition);
            }
            if (rule.Operator == "<")
            {
                var val = rule.Value.ToString();
                var date = DateTime.Parse(val).Date;
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.LessThan(
                    property,
                    Expression.Constant(date));
                left = Bind(left, right, rule.Condition);
            }
            if (rule.Operator == ">")
            {
                var val = rule.Value.ToString();
                var date = DateTime.Parse(val).Date;
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.GreaterThan(
                    property,
                    Expression.Constant(date));
                left = Bind(left, right, rule.Condition);
            }
            if (rule.Operator == ">=")
            {
                var val = rule.Value.ToString();
                var date = DateTime.Parse(val).Date;
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.GreaterThanOrEqual(
                    property,
                    Expression.Constant(date));
                left = Bind(left, right, rule.Condition);
            }
            if (rule.Operator == "<=")
            {
                var val = rule.Value.ToString();
                var date = DateTime.Parse(val).Date;
                var property = Expression.Property(parm, rule.Field);
                var right = Expression.LessThanOrEqual(
                    property,
                    Expression.Constant(date));
                left = Bind(left, right, rule.Condition);
            }
        }

        return left;
    }
    
    public Expression<Func<T, bool>> ParseExpressionOf<T>(QueryRule doc)
    {
        var itemExpression = Expression.Parameter(typeof(T));
        var conditions = ParseTree<T>(doc, itemExpression);

        var query = Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
        return query;
    }
    
}