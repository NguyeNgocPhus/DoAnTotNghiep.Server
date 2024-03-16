using DoAn.Shared.Services.V1.Identity.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.Identity.Validators;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().EmailAddress();
        RuleFor(command => command.Password)
            .NotEmpty();

    }
}