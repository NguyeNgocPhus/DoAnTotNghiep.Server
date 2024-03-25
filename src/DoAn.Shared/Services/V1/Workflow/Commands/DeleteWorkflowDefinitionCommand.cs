using DoAn.Shared.Abstractions.Messages;

namespace DoAn.Shared.Services.V1.Workflow.Commands;

public class DeleteWorkflowDefinitionCommand : ICommand
{
    public string DefinitionId { get; set; }
    public string Version { get; set; }
}