using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Identity.Responses;

namespace DoAn.Shared.Services.V1.Identity.Commands
{
    public class LogoutCommand : ICommand
    {
        public string Email { get; set; }
    }
}
