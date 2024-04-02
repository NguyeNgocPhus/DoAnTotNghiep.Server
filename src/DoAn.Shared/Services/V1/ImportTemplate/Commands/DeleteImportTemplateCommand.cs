using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Shared.Services.V1.ImportTemplate.Commands;

public class DeleteImportTemplateCommand : ICommand<DeleteImportTemplateResponse>
{
   public Guid Id { get; set; }
}