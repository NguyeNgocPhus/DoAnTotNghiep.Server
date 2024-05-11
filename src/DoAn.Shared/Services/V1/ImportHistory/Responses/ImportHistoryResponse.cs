namespace DoAn.Shared.Services.V1.ImportHistory.Responses;

public class ImportHistoryResponse 
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedTime { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedTime { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Guid? RoleProcessNextId { get; set; }
    public Guid ImportTemplateId { get; set; }
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public int Version { get; set; }
    public string Status { get; set; }
    public string CreatedByName { get; set; }
    public string ImportTemplateName { get; set; }
        
}