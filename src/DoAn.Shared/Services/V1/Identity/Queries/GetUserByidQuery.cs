using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Shared.Services.V1.Identity.Queries;

public class GetUserByIdQuery :  IQuery<UserResponse>
{
    public Guid Id { get; set; }
}