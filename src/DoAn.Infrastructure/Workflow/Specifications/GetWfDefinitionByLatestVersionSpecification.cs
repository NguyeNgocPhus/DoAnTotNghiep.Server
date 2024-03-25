using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.Specifications;

namespace DoAn.Infrastructure.Workflow.Specifications;

public class GetWfDefinitionByLatestVersionSpecification : Specification<WorkflowDefinition>
{
    public bool IsLatest { get; set; }

    public GetWfDefinitionByLatestVersionSpecification(bool isLatest)
    {
    
        IsLatest = isLatest;
    }

    public override Expression<Func<WorkflowDefinition, bool>> ToExpression() =>
        x =>  x.IsLatest == IsLatest;
}