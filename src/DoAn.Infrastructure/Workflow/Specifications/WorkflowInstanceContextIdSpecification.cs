using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.Specifications;

namespace DoAn.Infrastructure.Workflow.Specifications
{
    public class WorkflowInstanceContextIdSpecification : Specification<WorkflowInstance>
    {
        public string ContextId { get; set; }
        public WorkflowInstanceContextIdSpecification(string contextId) => ContextId = contextId;
        public override Expression<Func<WorkflowInstance, bool>> ToExpression() => x => x.ContextId == ContextId;
    }
}
