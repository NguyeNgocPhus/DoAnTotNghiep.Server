using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Queries;

namespace DoAn.Application.UseCases.V1.Queries.Workflow;

public class GetWorkflowActivityQueryHandler : IQueryHandler<GetWorkflowActivityQuery, object>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public GetWorkflowActivityQueryHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result<object>> Handle(GetWorkflowActivityQuery request, CancellationToken cancellationToken)
    {
        return  await _workflowDefinitionService.GetWorkflowActivityAsync(request.FileId, cancellationToken);
    }
}