using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.Workflow.Queries;

public class GetNodeDefinitionQuery : IQuery<NodeDefinitionResponse>
{
    public string DefinitionId { get; set; }
    public string Type { get; set; }
}