using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.ImportTemplate.Commands;

public class ImportDataCommand : ICommand
{
    public Guid ImportTemplateId { get; set; }
    public Guid FileUploadId { get; set; }
}