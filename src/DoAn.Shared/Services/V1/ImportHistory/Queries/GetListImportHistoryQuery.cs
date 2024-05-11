using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.ImportHistory.Responses;
using DoAn.Shared.Services.V1.ImportTemplate.Responses;

namespace DoAn.Shared.Services.V1.ImportHistory.Queries;

public class GetListImportHistoryQuery : PaginationBaseRequest ,IQuery<PagedResult<ImportHistoryResponse>>
{
    public List<Guid> ImportTemplateIds { get; set; } = [];
    public List<string> Status { get; set; } = [];
    public string? CreatedByName { get; set; }
}