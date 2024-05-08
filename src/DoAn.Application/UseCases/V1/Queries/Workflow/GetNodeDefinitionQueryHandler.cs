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
        var result =
            await _workflowDefinitionService.GetWorkflowDefinitionAsync(request.DefinitionId, cancellationToken);
        var activity = result.Activities.FirstOrDefault(x => x.ActivityId == request.ActivityId);
        if (activity == null)
        {
            return Result.Success(new NodeDefinitionResponse());
        }

        var propertyData = activity.Properties.FirstOrDefault(x => x.Name == "Data");
        if (propertyData == null)
        {
            return Result.Success(new NodeDefinitionResponse());
        }

        var propertyDescription = activity.Properties.FirstOrDefault(x => x.Name == "Description");


        var data = propertyData.Expressions.FirstOrDefault(x => x.Key == "Literal").Value;
        var description = propertyDescription?.Expressions.FirstOrDefault(x => x.Key == "Literal").Value ?? "";
        return data == null
            ? Result.Success(new NodeDefinitionResponse())
            : Result.Success(new NodeDefinitionResponse()
            {
                Data = data,
                Description = description
            });
    }
}