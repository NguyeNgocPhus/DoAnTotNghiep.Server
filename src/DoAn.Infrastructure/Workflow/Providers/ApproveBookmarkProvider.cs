using System.Runtime.CompilerServices;
using DoAn.Infrastructure.Workflow.Activities.Actions;
using DoAn.Infrastructure.Workflow.Bookmarks;
using Elsa.Services;

namespace DoAn.Infrastructure.Workflow.Providers;

public class ApproveBookmarkProvider : BookmarkProvider<ApproveBookmark, Approve>
{
    public override async ValueTask<IEnumerable<BookmarkResult>> GetBookmarksAsync(
        BookmarkProviderContext<Approve> context, CancellationToken cancellationToken) =>
        await GetBookmarksInternalAsync(context, cancellationToken).ToListAsync(cancellationToken);

    private async IAsyncEnumerable<BookmarkResult> GetBookmarksInternalAsync(BookmarkProviderContext<Approve> context,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var signalName = (await context.ReadActivityPropertyAsync(x => x.Signal, cancellationToken))?.ToLowerInvariant()
            .Trim();

        // Can't do anything with an empty signal name.
        if (string.IsNullOrEmpty(signalName))
            yield break;

        yield return Result(new ApproveBookmark
        {
            Signal = signalName
        });
    }
}