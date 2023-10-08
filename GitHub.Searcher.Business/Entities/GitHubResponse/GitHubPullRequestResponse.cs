using System.Text.Json.Serialization;

namespace GitHub.Searcher.Business.Entities.GitHubResponse;

public class GitHubPullRequestResponse
{
    [JsonPropertyName("html_url")]
    public Uri HtmlUrl { get; set; }
    public string State { get; set; }
    public string Title { get; set; }
    public GitHubUser User { get; set; }
    public GitHubLabel[]? Labels { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    public string Body { get; set; }
    public int Number { get; set; }
    public bool Draft { get; set; }
}