using DoAn.Application.Abstractions;
using DoAn.Shared.Abstractions.Messages;
using DoAn.Shared.Abstractions.Shared;
using DoAn.Shared.Services.V1.Workflow.Commands;

namespace DoAn.Application.UseCases.V1.Commands.Workflow;

public class ExecuteWorkflowCommandHandler : ICommandHandler<ExecuteWorkflowCommand>
{
    private readonly IWorkflowLaunchpadService _workflowLaunchpadService;

    public ExecuteWorkflowCommandHandler(IWorkflowLaunchpadService workflowLaunchpadService)
    {
        _workflowLaunchpadService = workflowLaunchpadService;
    }


    public async Task<Result> Handle(ExecuteWorkflowCommand request, CancellationToken cancellationToken)
    {
        var result = await _workflowLaunchpadService.ResumeWorkflowsAsync(request, cancellationToken);
        return result ? Result.Success(): Result.Failure(new Error("1","something wrong"));
    }
}