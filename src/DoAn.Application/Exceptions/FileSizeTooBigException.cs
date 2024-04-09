using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class FileSizeTooBigException : DomainException
{
    public FileSizeTooBigException(string message)
        : base("File size too big", message)
    {
    }
}