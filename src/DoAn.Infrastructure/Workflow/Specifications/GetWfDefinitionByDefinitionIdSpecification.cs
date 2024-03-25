using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.Specifications;

namespace DoAn.Infrastructure.Workflow.Specifications;

public class GetWfDefinitionByDefinitionIdSpecification : Specification<WorkflowDefinition>
{
    public string DefinitionId { get; set; }

    public GetWfDefinitionByDefinitionIdSpecification(string definitionId)
    {
    
        DefinitionId = definitionId;
    }

    public override Expression<Func<WorkflowDefinition, bool>> ToExpression() =>
        x =>  x.DefinitionId == DefinitionId;
}