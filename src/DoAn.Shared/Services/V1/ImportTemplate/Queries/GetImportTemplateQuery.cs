using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Shared.Services.V1.ImportTemplate.Queries;

public class GetImportTemplateQuery : IQuery<ImportTemplateResponse>
{
    public Guid Id { get; set; }
}