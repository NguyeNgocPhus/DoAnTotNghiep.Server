using DoAn.Application.Abstractions;
using DoAn.Application.DTOs.Workflow;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Queries;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Newtonsoft.Json.Linq;

namespace DoAn.Application.UseCases.V1.Queries.Workflow;

public class GetNodeDefinitionQueryHandler : IQueryHandler<GetNodeDefinitionQuery, NodeDefinitionResponse>
{
    private readonly IWorkflowDefinitionService _workflowDefinitionService;

    public GetNodeDefinitionQueryHandler(IWorkflowDefinitionService workflowDefinitionService)
    {
        _workflowDefinitionService = workflowDefinitionService;
    }

    public async Task<Result<NodeDefinitionResponse>> Handle(GetNodeDefinitionQuery request,
        CancellationToken cancellationToken)
    {
        var result =await _workflowDefinitionService.GetWorkflowDefinitionAsync(request.DefinitionId, cancellationToken);
        var activity = result.Activities.FirstOrDefault(x => x.Type == request.Type);
        if (activity == null)
        {
            return Result.Success(new NodeDefinitionResponse());
        }

        var property = activity.Properties.FirstOrDefault(x => x.Name == "DATA");
        if (property == null)
        {
            return Result.Success(new NodeDefinitionResponse());
        }

        var data = property.Expressions.FirstOrDefault(x => x.Key == "Literal").Value;
        return data == null ? Result.Success(new NodeDefinitionResponse()) : Result.Success(Newtonsoft.Json.JsonConvert.DeserializeObject<NodeDefinitionResponse>(data));
    }
}

