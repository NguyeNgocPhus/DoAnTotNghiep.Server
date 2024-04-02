using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.ImportTemplate.Commands;

public class UpdateImportTemplateCommand : ICommand<UpdateImportTemplateResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Tag { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
}