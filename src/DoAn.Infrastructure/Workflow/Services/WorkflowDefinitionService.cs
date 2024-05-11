using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Application.Exceptions;
using DoAn.Domain.Entities;
using DoAn.Domain.Entities.Identity;
using DoAn.Infrastructure.Workflow.Specifications;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Elsa;
using Elsa.Events;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Namotion.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Open.Linq.AsyncExtensions;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowDefinitionService : IWorkflowDefinitionService
{
    private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
    private readonly IWorkflowInstanceStore _workflowInstance;
    private readonly IWorkflowPublisher _workflowPublisher;
    private readonly IMapper _mapper;
    private readonly IActionLogRepository _actionLogRepository;
    private readonly IPublisher _publisher;
    private readonly RoleManager<AppRole> _roleManager;


    public WorkflowDefinitionService(IWorkflowDefinitionStore workflowDefinitionStore, IMapper mapper,
        IPublisher publisher, IWorkflowPublisher workflowPublisher, IWorkflowInstanceStore workflowInstance, IActionLogRepository actionLogRepository, RoleManager<AppRole> roleManager)
    {
        _workflowDefinitionStore = workflowDefinitionStore;
        _mapper = mapper;
        _publisher = publisher;
        _workflowPublisher = workflowPublisher;
        _workflowInstance = workflowInstance;
        _actionLogRepository = actionLogRepository;
        _roleManager = roleManager;
    }

    public async Task<CreateWorkflowResponse> CreateWorkflowDefinitionAsync(CreateWorkflowDefinitionCommand data,
        CancellationToken cancellationToken)
    {
        var workflowDefinition = new WorkflowDefinition()
        {
            Name = data.Name,
            DisplayName = data.Name,
            Description = data.Description,
            DefinitionId = Guid.NewGuid().ToString(),
            IsLatest = true,
            PersistenceBehavior = WorkflowPersistenceBehavior.WorkflowBurst
        };

        await _workflowDefinitionStore.SaveAsync(workflowDefinition, cancellationToken);

        return new CreateWorkflowResponse()
        {
            DefinitionId = workflowDefinition.DefinitionId,
            Name = data.Name,
            Version = workflowDefinition.Version,
            Description = data.Description
        };
    }

    public async Task<UpdateWorkflowDefinitionResponse> UpdateWorkflowDefinitionAsync(
        UpdateWorkflowDefinitionCommand data, CancellationToken cancellationToken = default)
    {
        var specification =
            new GetWfDefinitionByDefinitionIdAndVersionSpecification(data.Version, data.WorkflowDefinitionId);

        var wfDefinition = await _workflowDefinitionStore.FindAsync(specification, cancellationToken);
        if (wfDefinition == null)
        {
            throw new WorkflowDefinitionNotFoundException("workflow definition not found");
        }

        wfDefinition.Description = data.Description;
        wfDefinition.Name = data.Name;
        wfDefinition.IsPublished = true;
        wfDefinition.Activities = data.Activities.Select(x => new ActivityDefinition()
        {
            ActivityId = x.ActivityId,
            DisplayName = x.DisplayName,
            Name = x.DisplayName,
            Properties = x.Properties.Select(p => new ActivityDefinitionProperty()
            {
                Name = p.Name,
                Expressions = p.Expressions,
                Syntax = p.Syntax
            }).ToList(),
            Type = x.Type,
            PropertyStorageProviders = x.PropertyStorageProviders,
            PersistWorkflow = false,
            LoadWorkflowContext = false,
            SaveWorkflowContext = false
        }).ToList();
        wfDefinition.Connections = data.Connections.Select(x => new ConnectionDefinition()
        {
            SourceActivityId = x.SourceActivityId,
            TargetActivityId = x.TargetActivityId,
            Outcome = x.Outcome
        }).ToList();

        await _workflowDefinitionStore.UpdateAsync(wfDefinition, cancellationToken);

        await _publisher.Publish(new WorkflowDefinitionPublished(wfDefinition), cancellationToken);
        await _publisher.Publish(new WorkflowDefinitionSaved(wfDefinition), cancellationToken);

        return _mapper.Map<UpdateWorkflowDefinitionResponse>(data);
    }

    public async Task<string> DeleteWorkflowDefinitionAsync(DeleteWorkflowDefinitionCommand data,
        CancellationToken cancellationToken = default)
    {
        var specification = new GetWfDefinitionByDefinitionIdSpecification(data.DefinitionId);
        await _workflowDefinitionStore.DeleteManyAsync(specification, cancellationToken);
        return data.DefinitionId;
    }

    public async Task<List<WorkflowDefinitionResponse>> GetListWorkflowDefinitionAsync(
        CancellationToken cancellationToken = default)
    {
        var worklows = await _workflowDefinitionStore.FindManyAsync(
            new GetWfDefinitionByLatestVersionSpecification(true),
            cancellationToken: cancellationToken).ToList();


        return _mapper.Map<List<WorkflowDefinitionResponse>>(worklows);
    }

    public async Task<WorkflowDefinitionResponse> GetWorkflowDefinitionAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var result =
            await _workflowDefinitionStore.FindByDefinitionIdAsync(id, VersionOptions.Latest, cancellationToken);
        if (result == null)
            throw new WorkflowDefinitionNotFoundException("\"workflow definition not found\"");
        return _mapper.Map<WorkflowDefinitionResponse>(result);
    }

    public async Task<object> GetWorkflowActivityAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var workflowInstance =
            await _workflowInstance.FindAsync(new WorkflowInstanceContextIdSpecification(fileId.ToString()),
                cancellationToken);

        var workflowDefinition = await _workflowDefinitionStore.FindByDefinitionIdAsync(workflowInstance.DefinitionId,
            VersionOptions.SpecificVersion(workflowInstance.Version), cancellationToken: cancellationToken);

        // if root activity 
        var rootActivity = workflowDefinition.Connections.First(x =>
            workflowDefinition.Connections.FirstOrDefault(y => y.TargetActivityId == x.SourceActivityId) == null);


        int node = 1;
        List<KeyValuePair<int, ActivityDefinition>> dictionary = new List<KeyValuePair<int, ActivityDefinition>>();

        var activity =
            workflowDefinition.Activities.FirstOrDefault(x => x.ActivityId == rootActivity.SourceActivityId);
        dictionary.Add(new KeyValuePair<int, ActivityDefinition>(node, activity));


        // create workflow tree 
         GenerateWorkflowTree(rootActivity.TargetActivityId, node + 1);

        async void GenerateWorkflowTree(string targetActivityId, int node)
        {
            var connections = workflowDefinition?.Connections.Where(x => x.SourceActivityId == targetActivityId);
            if (connections.Count() == 0)
            {
                return;
            }
            else
            {
                foreach (var connect in connections)
                {
                    var activity =
                        workflowDefinition?.Activities.FirstOrDefault(x =>
                            x.ActivityId == connect.SourceActivityId);
                    if (activity != null && activity.Type != "Branch" && activity.Type != "Condition" &&
                        activity.Type != "Join" && activity.Type != "SendEmail")
                    {
                        if (activity.Type is "Approve" or "Reject")
                        {
                            var properties = activity.Properties.FirstOrDefault(x => x.Name == "Data");
                            var value = properties.Expressions.FirstOrDefault(x=>x.Key=="Literal").Value;
                            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(value);
                            var roleId = obj["roleId"].ToString();
                            var role =  _roleManager.FindByIdAsync(roleId).Result;
                            activity.Description = $"Chờ {role.Name} phê duyệt";
                        }
                        
                        dictionary.Add(new KeyValuePair<int, ActivityDefinition>(node, activity));
                    }

                    GenerateWorkflowTree(connect.TargetActivityId, node + 1);
                }
            }
        }

        var activities = dictionary.GroupBy(x => x.Key, (key, g) => g.ToList().Select(i => i.Value));


        var actionLogs = await _actionLogRepository.GetActionLog(fileId, cancellationToken);

        return new {
            activities,
            actionLogs
        };
    }
}