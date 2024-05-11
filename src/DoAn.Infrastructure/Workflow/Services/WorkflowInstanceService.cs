using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities.Identity;
using DoAn.Infrastructure.Workflow.Specifications;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Elsa;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowInstanceService : IWorkflowInstanceService
{
    private readonly IWorkflowLaunchpad _workflowLaunchpad;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IWorkflowInstanceStore _workflowInstance;
    private readonly IWorkflowDefinitionStore _workflowDefinition;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public WorkflowInstanceService(IWorkflowLaunchpad workflowLaunchpad, IHttpContextAccessor httpContext,
        IWorkflowInstanceStore workflowInstance, IWorkflowDefinitionStore workflowDefinition,
        UserManager<AppUser> userManager, ICurrentUserService currentUserService, RoleManager<AppRole> roleManager, IUserRepository userRepository)
    {
        _workflowLaunchpad = workflowLaunchpad;
        _httpContext = httpContext;
        _workflowInstance = workflowInstance;
        _workflowDefinition = workflowDefinition;
        _userManager = userManager;
        _currentUserService = currentUserService;
        _roleManager = roleManager;
        _userRepository = userRepository;
    }

    public async Task<CurrentStepWorkflowResponse> GetCurrentStepAsync(Guid fileId,
        CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var roleIds =  await _userRepository.GetRoleIdsInUser(currentUser.Id, cancellationToken);
        
       

        var workflowInstance =
            await _workflowInstance.FindAsync(new WorkflowInstanceContextIdSpecification(fileId.ToString()),
                cancellationToken);

        if (workflowInstance == null)
            throw new WorkflowInstanceNotFoundException("Workflow instance not found");
        var blockActivity = workflowInstance.BlockingActivities;

        // get wf definition by definition id
        var workflowDefinition = await _workflowDefinition.FindByDefinitionIdAsync(workflowInstance.DefinitionId,
            VersionOptions.Latest, cancellationToken: cancellationToken);

        var activitiesDefinition =
            workflowDefinition!.Activities.Where(x => x.Properties.Any(xx => xx.Name == "Signal"));

        // current activities are blocking
        var currentBlockingActivities = activitiesDefinition
            .Where(x => blockActivity.Select(r => r.ActivityId).Contains(x.ActivityId)).ToList();
        // get activities assign to current user
        var hasApprove = currentBlockingActivities
            .Any(x =>
                {
                    var value = x.Properties.First(f => f.Name == "Data").Expressions.First().Value;
                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(value);
                    var roleId = obj["roleId"].ToString();
                    return roleIds.Contains(roleId);
                }
            );
        if (!hasApprove)
        {
            currentBlockingActivities = new List<ActivityDefinition>();
        }

        return new CurrentStepWorkflowResponse()
        {
            WorkflowInstanceId = workflowInstance.Id,
            Activities = currentBlockingActivities.Select(x => new CurrentActivity()
            {
                Type = x.Type,
                ActivityId = x.ActivityId,
                Signal = x.Properties.First(p => p.Name == "Signal").Expressions.First(x => x.Key == "Literal").Value
            }).ToList()
        };
    }
}