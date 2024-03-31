using DoAn.Shared.Services.V1.Workflow.Common;

namespace DoAn.Shared.Services.V1.Workflow.Responses;

public class WorkflowDefinitionResponse
{
    public string Id { get; set; }
    public string DefinitionId { get; set; }
    public bool Publish { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public int Version { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Connection> Connections { get; set; }
}