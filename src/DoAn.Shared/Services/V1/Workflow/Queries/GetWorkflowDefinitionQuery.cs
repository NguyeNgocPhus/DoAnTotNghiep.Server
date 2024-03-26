using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.Workflow.Queries;

public class GetWorkflowDefinitionQuery : IQuery<WorkflowDefinitionResponse>
{
    public string DefinitionId { get; set; }
}