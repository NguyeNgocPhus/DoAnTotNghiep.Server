using Elsa.Services;

namespace DoAn.Infrastructure.Workflow.Bookmarks;

public class FileUploadBookmark : IBookmark
{
    public string Data { get; set; } = default!;

    public FileUploadBookmark(string data)
    {
        Data = data;
    }
}