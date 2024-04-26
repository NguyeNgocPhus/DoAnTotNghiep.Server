using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Identity.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Shared.Services.V1.Identity.Commands
{
    public class UpdateUserCommand : ICommand<UserResponse>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
