namespace DoAn.Shared.Services.V1.ImportTemplate.Responses;

public class ImportTemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
      
    public string Description { get; set; }
    public bool Active { get; set; }
    public bool HasWorkflow { get; set; }
    public string? WorkflowDefinitionId { get; set; }
    public int DisplayOrder { get; set; }
    public Guid FileTemplateId { get; set; }

}