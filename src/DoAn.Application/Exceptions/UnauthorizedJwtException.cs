using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class UnauthorizedJwtException: DomainException
{
    public UnauthorizedJwtException(string title, string message) : base(title, message)
    {
    }
}