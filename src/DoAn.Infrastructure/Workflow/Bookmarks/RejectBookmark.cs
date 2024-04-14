using Elsa.Services;

namespace DoAn.Infrastructure.Workflow.Bookmarks;

public class RejectBookmark : IBookmark
{
    public string Signal { get; set; } = default!;
}