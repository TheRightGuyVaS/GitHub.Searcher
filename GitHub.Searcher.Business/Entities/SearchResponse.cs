namespace GitHub.Searcher.Business.Entities;

public class SearchResponse
{
    private static readonly PullRequestModelComparer Comparer = new PullRequestModelComparer();
    
    public SortedSet<PullRequestModel> Active { get; set; }
    public SortedSet<PullRequestModel> Draft { get; set; }
    public SortedSet<PullRequestModel> Stale { get; set; }

    public SearchResponse()
    {
        Active = new SortedSet<PullRequestModel>(Comparer);
        Draft = new SortedSet<PullRequestModel>(Comparer);
        Stale = new SortedSet<PullRequestModel>(Comparer);
    }
}

internal class PullRequestModelComparer : IComparer<PullRequestModel>
{
    public int Compare(PullRequestModel? x, PullRequestModel? y)
    {
        if (x == null || y == null)
        {
            return 0;
        }

        return x.Number.CompareTo(y.Number);
    }
}