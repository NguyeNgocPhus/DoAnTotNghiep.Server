using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class WorkflowDefinitionNotFoundException : NotFoundException
{
    public WorkflowDefinitionNotFoundException(string message) : base(message)
    {
    }
}