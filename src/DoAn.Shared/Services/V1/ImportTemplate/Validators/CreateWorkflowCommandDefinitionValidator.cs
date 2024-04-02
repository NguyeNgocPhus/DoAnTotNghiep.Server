using DoAn.Shared.Services.V1.Workflow.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.ImportTemplate.Validators;

public class CreateImportTemplateCommandValidator: AbstractValidator<CreateWorkflowDefinitionCommand>
{
    public CreateImportTemplateCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty();

    }
}