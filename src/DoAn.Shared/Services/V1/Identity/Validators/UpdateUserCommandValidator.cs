using DoAn.Shared.Services.V1.Identity.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Shared.Services.V1.Identity.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty().EmailAddress();
            RuleFor(command => command.PhoneNumber)
                .NotEmpty();
            RuleFor(command => command.UserName)
                .NotEmpty();
            // RuleFor(command => command.LastName)
            //     .NotEmpty();

        }
    }
}
