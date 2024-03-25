using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.UseCases.V1.Commands.Workflow;

public class CreateWorkflowCommandHandler : ICommandHandler<CreateWorkflowDefinitionCommand, CreateWorkflowResponse>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public CreateWorkflowCommandHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result<CreateWorkflowResponse>> Handle(CreateWorkflowDefinitionCommand request, CancellationToken cancellationToken)
    {
        var result = await _workflowDefinitionService.CreateWorkflowDefinitionAsync(request, cancellationToken);
        return Result.Success(result);
    }
}