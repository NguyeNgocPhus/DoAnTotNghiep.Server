using AutoMapper;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Common;
using DoAn.Shared.Services.V1.Workflow.Responses;
using Elsa.Models;

namespace DoAn.Infrastructure.Mapper;

public class WorkflowProfile: Profile
{
    public WorkflowProfile()
    {
        CreateMap<UpdateWorkflowDefinitionCommand, UpdateWorkflowDefinitionResponse>();
        // CreateMap<ActivityDefinition, Activity>();
        // CreateMap<ActivityDefinitionProperty, ActivityProperty>();
        // CreateMap<ConnectionDefinition, Connection>();
        CreateMap<WorkflowDefinition, WorkflowDefinitionResponse>()
            .ForMember(x => x.Connections, opt => opt.Ignore())
            .ForMember(x => x.Activities, opt => opt.Ignore());
    }
}