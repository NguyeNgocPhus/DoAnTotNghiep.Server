using DoAn.Shared.Services.V1.Identity.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Shared.Services.V1.Identity.Validators
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().EmailAddress();
        }
    }
}
