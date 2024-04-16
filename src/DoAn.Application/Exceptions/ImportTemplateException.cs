using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class ImportTemplateException : NotFoundException
{
    public ImportTemplateException(string message) : base(message)
    {
    }
}