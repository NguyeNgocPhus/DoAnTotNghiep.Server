using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;
using Elsa.Persistence;

namespace DoAn.Application.UseCases.V1.Commands.Workflow;

public class ExecuteWorkflowCommandHandler : ICommandHandler<ExecuteWorkflowCommand>
{
    private readonly IWorkflowLaunchpadService _workflowLaunchpadService;
    private readonly IWorkflowInstanceStore _workflowInstance;

    public ExecuteWorkflowCommandHandler(IWorkflowLaunchpadService workflowLaunchpadService, IWorkflowInstanceStore workflowInstance)
    {
        _workflowLaunchpadService = workflowLaunchpadService;
        _workflowInstance = workflowInstance;
    }


    public async Task<Result> Handle(ExecuteWorkflowCommand request, CancellationToken cancellationToken)
    {
        
        var result = await _workflowLaunchpadService.ResumeWorkflowsAsync(request, cancellationToken);
        return Result.Success(result);
    }
}