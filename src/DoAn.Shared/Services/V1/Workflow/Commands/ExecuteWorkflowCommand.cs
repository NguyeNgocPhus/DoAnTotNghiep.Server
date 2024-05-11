using DoAn.Shared.Abstractions.Messages;

namespace DoAn.Shared.Services.V1.Workflow.Commands;

public class ExecuteWorkflowCommand : ICommand
{
    public string Signal { get; set; }
    public string WorkflowInstanceId { get; set; }
    public string ActivityId { get; set; }
    public string RejectReason { get; set; }
} 