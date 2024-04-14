using Elsa.Services;

namespace DoAn.Infrastructure.Workflow.Bookmarks;

public class ApproveBookmark : IBookmark
{
    public string Signal { get; set; } = default!;
}