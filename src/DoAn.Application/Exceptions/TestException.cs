namespace DoAn.Application.Exceptions;

public class TestException : Exception
{
    public TestException(string title, string message)
        : base(message) =>
        Title = title;

    public string Title { get; }
}