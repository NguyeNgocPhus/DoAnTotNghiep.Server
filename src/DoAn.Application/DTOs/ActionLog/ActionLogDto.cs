using DoAn.Domain.Abstractions;

namespace DoAn.Application.DTOs.ActionLog;

public class ActionLogDto : Entity<Guid>
{
    public string WorkflowDefinitionId { get; set; }

    public string WorkflowInstanceId { get; set; }

    public Guid ContextId { get; set; }
    public string ActivityId { get; set; }

    public string ActivityName { get; set; }
    public string? ActionReason { get; set; }
    public string CreatedByName { get; set; }
 
}