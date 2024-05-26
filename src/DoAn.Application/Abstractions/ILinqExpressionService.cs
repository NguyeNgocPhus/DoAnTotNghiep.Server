using System.Linq.Expressions;
using DoAn.Application.DTOs;

namespace DoAn.Application.Abstractions;

public interface ILinqExpressionService
{
    public Expression BuildExpression(QueryRule root, ParameterExpression parm);
    
}