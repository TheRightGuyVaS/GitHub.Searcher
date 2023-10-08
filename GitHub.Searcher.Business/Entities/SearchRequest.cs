namespace GitHub.Searcher.Business.Entities;

public class SearchRequest
{
    public string RepositoryOwner { get; set; }
    public string RepositoryName { get; set; }
    public string PullRequestLabel { get; set; }
    public IReadOnlyCollection<string> Keywords { get; set; }
}