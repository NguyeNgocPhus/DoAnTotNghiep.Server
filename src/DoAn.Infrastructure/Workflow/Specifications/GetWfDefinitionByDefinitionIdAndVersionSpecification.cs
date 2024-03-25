using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.Specifications;

namespace DoAn.Infrastructure.Workflow.Specifications;

public class GetWfDefinitionByDefinitionIdAndVersionSpecification : Specification<WorkflowDefinition>
{
    public int Version { get; set; }
    public string DefinitionId { get; set; }

    public GetWfDefinitionByDefinitionIdAndVersionSpecification(int version, string definitionId)
    {
        Version = version;
        DefinitionId = definitionId;
    }

    public override Expression<Func<WorkflowDefinition, bool>> ToExpression() =>
        x => x.Version == Version && x.DefinitionId == DefinitionId;
}