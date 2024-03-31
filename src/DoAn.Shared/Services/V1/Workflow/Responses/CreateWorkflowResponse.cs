namespace DoAn.Shared.Services.V1.Workflow.Responses;

public class CreateWorkflowResponse
{
    public string DefinitionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Version { get; set; }
}