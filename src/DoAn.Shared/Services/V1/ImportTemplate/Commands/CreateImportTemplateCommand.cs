using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Shared.Services.V1.ImportTemplate.Commands;

public class CreateImportTemplateCommand : ICommand<CreateImportTemplateResponse>
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public string Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    
}