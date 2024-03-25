using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;


namespace DoAn.Application.UseCases.V1.Commands.Workflow;

public class
    UpdateWorkflowDefinitionCommandHandler : ICommandHandler<UpdateWorkflowDefinitionCommand,
    UpdateWorkflowDefinitionResponse>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public UpdateWorkflowDefinitionCommandHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result<UpdateWorkflowDefinitionResponse>> Handle(UpdateWorkflowDefinitionCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _workflowDefinitionService.UpdateWorkflowDefinitionAsync(request, cancellationToken);
        return Result.Success(result);
    }
}