using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Shared.Services.V1.Identity.Queries;

public class GetListUserQuery : PaginationBaseRequest, IQuery<PagedResult<UserResponse>>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string?  PhoneNumber { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}