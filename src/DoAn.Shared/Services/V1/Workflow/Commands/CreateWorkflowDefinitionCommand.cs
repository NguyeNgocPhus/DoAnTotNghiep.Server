using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Shared.Services.V1.Workflow.Commands;

public class CreateWorkflowDefinitionCommand : ICommand<CreateWorkflowResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
}