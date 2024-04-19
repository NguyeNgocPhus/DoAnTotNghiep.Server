using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Shared.Services.V1.Identity.Commands;

public class CreateUserCommand : ICommand<UserResponse>
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}