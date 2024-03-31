using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;

namespace DoAn.Application.UseCases.V1.Commands.Workflow;

public class DeleteWorkflowDefinitionCommandHandler : ICommandHandler<DeleteWorkflowDefinitionCommand>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public DeleteWorkflowDefinitionCommandHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result> Handle(DeleteWorkflowDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definitionId = await _workflowDefinitionService.DeleteWorkflowDefinitionAsync(request, cancellationToken);
        return Result.Success(definitionId);
    }
}