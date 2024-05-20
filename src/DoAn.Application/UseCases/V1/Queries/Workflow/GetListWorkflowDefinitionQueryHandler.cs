using DoAn.Application.Abstractions;
using DoAn.Application.Websocket;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Queries;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Application.UseCases.V1.Queries.Workflow;

public class GetListWorkflowDefinitionQueryHandler : IQueryHandler<GetListWorkflowDefinitionQuery,List<WorkflowDefinitionResponse>>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;
    private readonly WebsocketHandler _ws;
    private readonly ICurrentUserService _currentUserService;

    public GetListWorkflowDefinitionQueryHandler(IWorkflowDefinitionService workflowDefinitionService, WebsocketHandler ws, ICurrentUserService currentUserService)
    {
        _workflowDefinitionService = workflowDefinitionService;
        _ws = ws;
        _currentUserService = currentUserService;
    }

    public async Task<Result<List<WorkflowDefinitionResponse>>> Handle(GetListWorkflowDefinitionQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var result = await _workflowDefinitionService.GetListWorkflowDefinitionAsync(request, cancellationToken);
        // await _ws.SendMessageToClient(userId, "hello");
        return Result.Success(result) ;
    }
}