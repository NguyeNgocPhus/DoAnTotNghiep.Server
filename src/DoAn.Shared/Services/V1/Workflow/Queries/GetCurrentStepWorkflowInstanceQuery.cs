using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.Workflow.Queries;

public class GetCurrentStepWorkflowInstanceQuery : IQuery<CurrentStepWorkflowResponse>
{
    public Guid FileId { get; set; }
}