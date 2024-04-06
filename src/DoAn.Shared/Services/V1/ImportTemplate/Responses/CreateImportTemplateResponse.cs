namespace DoAn.Shared.Services.V1.ImportTemplate.Responses;

public class CreateImportTemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; }
    public string Tag { get; set; }
    public bool Active { get; set; }

}