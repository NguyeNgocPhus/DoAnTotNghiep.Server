namespace DoAn.Domain.Extensions;

public class NotFoundException : DomainException
{
    protected NotFoundException(string message)
        : base("Not found", message)
    {
    }
}