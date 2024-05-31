using DoAn.Domain.Abstractions;

namespace DoAn.Domain.Entities;

public class ActionLogs: AuditableEntity<Guid>
{
   
    public string WorkflowDefinitionId { get; set; }

    public string WorkflowInstanceId { get; set; }

    public Guid ContextId { get; set; }
    public string ActivityId { get; set; }

    public string ActivityName { get; set; }
    public string? ActionReason { get; set; }
    public string? Data { get; set; }
    
}