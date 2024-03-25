using DoAn.Shared.Services.V1.Workflow.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.Workflow.Validators;

public class CreateWorkflowDefinitionCommandValidator: AbstractValidator<CreateWorkflowDefinitionCommand>
{
    public CreateWorkflowDefinitionCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty();

    }
}