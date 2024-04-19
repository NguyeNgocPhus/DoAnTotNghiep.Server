using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class LoginFailException: DomainException
{
    public LoginFailException(string title, string message) : base(title, message)
    {
    }
}