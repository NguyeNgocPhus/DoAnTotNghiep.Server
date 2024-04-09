using DoAn.Shared.Services.V1.FileStorage.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.FileStorage.Validators;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        // RuleFor(command => command.Email)
        //     .NotEmpty().EmailAddress();
        // RuleFor(command => command.Password)
        //     .NotEmpty();

    }
}