using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Queries;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.UseCases.V1.Queries.Workflow;

public class GetListWorkflowDefinitionQueryHandler : IQueryHandler<GetListWorkflowDefinitionQuery,List<WorkflowDefinitionResponse>>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public GetListWorkflowDefinitionQueryHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result<List<WorkflowDefinitionResponse>>> Handle(GetListWorkflowDefinitionQuery request, CancellationToken cancellationToken)
    {
        var result = await _workflowDefinitionService.GetListWorkflowDefinitionAsync(cancellationToken);
        return Result.Success(result);
    }
}