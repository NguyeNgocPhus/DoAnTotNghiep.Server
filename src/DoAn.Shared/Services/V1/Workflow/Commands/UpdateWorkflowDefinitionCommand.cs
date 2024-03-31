using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Workflow.Common;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.Workflow.Commands;

public class UpdateWorkflowDefinitionCommand : ICommand<UpdateWorkflowDefinitionResponse>
{
    public string WorkflowDefinitionId { get; set; }
    public bool Publish { get; set; }
    public string Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public int Version { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Connection> Connections { get; set; }
}