using System.Linq.Expressions;
using Elsa.Models;
using Elsa.Persistence.Specifications;

namespace DoAn.Infrastructure.Workflow.Specifications
{
    public class WorkflowExecutionLogRecordActivityIdSpecification : Specification<WorkflowExecutionLogRecord>
    {
        public string ActivityId { get; set; }
        public WorkflowExecutionLogRecordActivityIdSpecification(string activityId) => ActivityId = activityId;
        public override Expression<Func<WorkflowExecutionLogRecord, bool>> ToExpression() => x => x.ActivityId == ActivityId;
    }
}
