using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Identity.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Shared.Services.V1.Identity.Commands
{
    public class DeleteUserCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
