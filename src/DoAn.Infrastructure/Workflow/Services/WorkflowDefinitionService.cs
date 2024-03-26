using AutoMapper;
using DoAn.Application.Abstractions;
using DoAn.Application.Exceptions;
using DoAn.Infrastructure.Workflow.Specifications;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Elsa;
using Elsa.Models;
using Elsa.Persistence;
using Namotion.Reflection;
using Open.Linq.AsyncExtensions;

namespace DoAn.Infrastructure.Workflow.Services;

public class WorkflowDefinitionService : IWorkflowDefinitionService
{
    private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
    private readonly IMapper _mapper;

    public WorkflowDefinitionService(IWorkflowDefinitionStore workflowDefinitionStore, IMapper mapper)
    {
        _workflowDefinitionStore = workflowDefinitionStore;
        _mapper = mapper;
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
        return _mapper.Map<UpdateWorkflowDefinitionResponse>(data);
    }

    public async Task<bool> DeleteWorkflowDefinitionAsync(DeleteWorkflowDefinitionCommand data,
        CancellationToken cancellationToken = default)
    {
        var specification = new GetWfDefinitionByDefinitionIdSpecification(data.DefinitionId);
        var row = await _workflowDefinitionStore.DeleteManyAsync(specification, cancellationToken);
        return row > 0;
    }

    public async Task<List<WorkflowDefinitionResponse>> GetListWorkflowDefinitionAsync(
        CancellationToken cancellationToken = default)
    {
        var worklows = await _workflowDefinitionStore.FindManyAsync(new GetWfDefinitionByLatestVersionSpecification(true),
            cancellationToken: cancellationToken).ToList();


        return _mapper.Map<List<WorkflowDefinitionResponse>>(worklows);
    }

    public async Task<WorkflowDefinitionResponse> GetWorkflowDefinitionAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _workflowDefinitionStore.FindByDefinitionIdAsync( id,VersionOptions.Latest, cancellationToken);
        if (result == null)
            throw new WorkflowDefinitionNotFoundException("\"workflow definition not found\"");
        return _mapper.Map<WorkflowDefinitionResponse>(result);
    }   
}