using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Shared.Services.V1.Identity.Queries;

public class GetListUserQuery : PaginationBaseRequest, IQuery<PagedResult<UserResponse>>
{
}