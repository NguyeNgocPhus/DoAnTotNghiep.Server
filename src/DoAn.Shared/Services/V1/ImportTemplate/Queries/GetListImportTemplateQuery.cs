using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Shared.Services.V1.ImportTemplate.Queries;

public class GetListImportTemplateQuery : PaginationBaseRequest, IQuery<PagedResult<ImportTemplateResponse>>
{
    public string? Name { get; set; }
    public string? Tag { get; set; }
}