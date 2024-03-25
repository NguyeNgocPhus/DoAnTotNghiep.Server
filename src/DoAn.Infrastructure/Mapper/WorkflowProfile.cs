using AutoMapper;
using DoAn.Shared.Services.V1.Workflow.Commands;
using DoAn.Shared.Services.V1.Workflow.Responses;

namespace DoAn.Infrastructure.Mapper;

public class WorkflowProfile: Profile
{
    public WorkflowProfile()
    {
        CreateMap<UpdateWorkflowDefinitionCommand, UpdateWorkflowDefinitionResponse>();
    }
}