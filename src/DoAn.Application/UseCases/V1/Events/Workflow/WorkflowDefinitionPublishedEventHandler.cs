using DoAn.Application.Abstractions;
using DoAn.Application.Abstractions.Repositories;
using DoAn.Domain.Entities;
using DoAn.Shared.Extensions;
using Elsa.Events;
using MediatR;

namespace DoAn.Application.UseCases.V1.Events.Workflow;

public class WorkflowDefinitionPublishedEventHandler : INotificationHandler<WorkflowDefinitionPublished>

{
    private readonly IRepositoryBase<ImportTemplate, Guid> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public WorkflowDefinitionPublishedEventHandler(IRepositoryBase<ImportTemplate, Guid> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public class Data
    {
        public string ImportTemplateId { get; set; }
    }
    public async Task Handle(WorkflowDefinitionPublished notification, CancellationToken cancellationToken)
    {
        var property = notification.WorkflowDefinition.Activities.FirstOrDefault(x => x.Type == "FileUpload")
            ?.Properties.Where(x => x.Name == "Data")
            .Select(x => x.Expressions.FirstOrDefault(e => e.Key == "Literal").Value);
        var data = property?.FirstOrDefault();
        if (data is not null)
        {
            
            var formatData = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(data);
            if (formatData.ImportTemplateId != null)
            {
                var importTemplate =
                    await _repository.FindByIdAsync(Guid.Parse(formatData.ImportTemplateId), cancellationToken);
                importTemplate.WorkflowDefinitionId = notification.WorkflowDefinition.Id;
                importTemplate.HasWorkflow = true;
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}