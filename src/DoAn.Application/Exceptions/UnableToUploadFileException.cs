using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class UnableToUploadFileException: DomainException
{
public UnableToUploadFileException(string message)
    : base("Can`t upload", message)
{
}
}