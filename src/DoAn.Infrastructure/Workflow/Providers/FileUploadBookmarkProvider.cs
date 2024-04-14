using DoAn.Infrastructure.Workflow.Activities.Triggers;
using DoAn.Infrastructure.Workflow.Bookmarks;
using Elsa.Services;

namespace DoAn.Infrastructure.Workflow.Providers;

public class UploadFileBookmarkProvider: BookmarkProvider<FileUploadBookmark, FileUpload>
{
    public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(BookmarkProviderContext<FileUpload> context, CancellationToken cancellationToken)
    {
        var supportedExtensions = (await context.ReadActivityPropertyAsync<FileUpload, string>(x => x.Data, cancellationToken)) ?? string.Empty;

        return new[] { Result(new FileUploadBookmark(supportedExtensions)) };
    }
}