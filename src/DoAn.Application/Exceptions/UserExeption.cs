using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class DuplicateUserException : DomainException
{
    public DuplicateUserException(string message) : base("Duplicate", message)
    {
    }
}