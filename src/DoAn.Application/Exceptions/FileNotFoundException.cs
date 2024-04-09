using DoAn.Domain.Extensions;

namespace DoAn.Application.Exceptions;

public class FileNotFoundException: NotFoundException
{
    public FileNotFoundException(string message) : base(message)
    {
    }
}