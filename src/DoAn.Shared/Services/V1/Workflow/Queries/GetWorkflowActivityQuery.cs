using DoAn.Shared.Abstractions.Messages;

namespace DoAn.Shared.Services.V1.Workflow.Queries;

public class GetWorkflowActivityQuery : IQuery<object>
{
    public Guid FileId { get; set; } 
}