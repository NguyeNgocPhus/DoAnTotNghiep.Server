using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class UnAllowedFileException : DomainException
{
    protected UnAllowedFileException( string message) : base("UnAllow File", message)
    {
    }
}