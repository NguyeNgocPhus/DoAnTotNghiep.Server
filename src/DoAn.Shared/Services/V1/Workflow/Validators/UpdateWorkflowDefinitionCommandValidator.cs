using DoAn.Shared.Services.V1.Workflow.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.Workflow.Validators;

public class UpdateWorkflowDefinitionCommandValidator: AbstractValidator<UpdateWorkflowDefinitionCommand>
{
    public UpdateWorkflowDefinitionCommandValidator()
    {
        // RuleFor(command => command.Name)
        //     .NotEmpty();

    }
}