using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Queries;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.UseCases.V1.Queries.Workflow;

public class
    GetCurrentStepWorkflowInstanceQueryHandler : IQueryHandler<GetCurrentStepWorkflowInstanceQuery,
    CurrentStepWorkflowResponse>
{
    private readonly IWorkflowInstanceService _workflowInstanceService;

    public GetCurrentStepWorkflowInstanceQueryHandler(IWorkflowInstanceService workflowInstanceService)
    {
        _workflowInstanceService = workflowInstanceService;
    }

    public async Task<Result<CurrentStepWorkflowResponse>> Handle(GetCurrentStepWorkflowInstanceQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _workflowInstanceService.GetCurrentStepAsync(request.FileId, cancellationToken);
        return Result.Success(result);
    }
}