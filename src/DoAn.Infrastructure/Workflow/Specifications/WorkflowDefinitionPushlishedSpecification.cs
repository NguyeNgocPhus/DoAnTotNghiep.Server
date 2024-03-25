using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.Specifications;

namespace DoAn.Infrastructure.Workflow.Specifications;
public class WorkflowDefinitionPublishedSpecification : Specification<WorkflowDefinition>
{
    public bool IsPublished { get; set; }
    public WorkflowDefinitionPublishedSpecification(bool isPublished) => IsPublished = isPublished;
    public override Expression<Func<WorkflowDefinition, bool>> ToExpression() => x => x.IsPublished == IsPublished;
}