using DoAn.Shared.Services.V1.Workflow.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.Workflow.Validators;

public class CreateWorkflowCommandValidator: AbstractValidator<CreateWorkflowCommand>
{
    public CreateWorkflowCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty();

    }
}