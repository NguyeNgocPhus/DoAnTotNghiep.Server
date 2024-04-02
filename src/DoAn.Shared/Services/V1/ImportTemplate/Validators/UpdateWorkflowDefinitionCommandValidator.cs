using DoAn.Shared.Services.V1.Workflow.Commands;
using FluentValidation;

namespace DoAn.Shared.Services.V1.ImportTemplate.Validators;

public class UpdateImportTemplateCommandValidator: AbstractValidator<UpdateWorkflowDefinitionCommand>
{
    public UpdateImportTemplateCommandValidator()
    {
        // RuleFor(command => command.Name)
        //     .NotEmpty();

    }
}