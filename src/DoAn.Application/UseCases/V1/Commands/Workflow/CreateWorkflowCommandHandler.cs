using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.UseCases.V1.Commands.Workflow;

public class CreateWorkflowCommandHandler : ICommandHandler<CreateWorkflowCommand, CreateWorkflowResponse>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public CreateWorkflowCommandHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result<CreateWorkflowResponse>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        var result = await _workflowDefinitionService.CreateWorkflowAsync(request);
        return result;
    }
}