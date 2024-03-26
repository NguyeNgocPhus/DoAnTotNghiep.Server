using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Queries;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.UseCases.V1.Queries.Workflow;

public class GetWorkflowDefinitionQueryHandler : IQueryHandler<GetWorkflowDefinitionQuery,WorkflowDefinitionResponse>
{
    
    
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public GetWorkflowDefinitionQueryHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }
    public async Task<Result<WorkflowDefinitionResponse>> Handle(GetWorkflowDefinitionQuery request, CancellationToken cancellationToken)
    {
        var result =await _workflowDefinitionService.GetWorkflowDefinitionAsync(request.DefinitionId, cancellationToken);
        return Result.Success(result);
    }
}