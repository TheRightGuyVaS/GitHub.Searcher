using System.Text.Json.Serialization;

namespace GitHub.Searcher.Business.Entities.GitHubResponse;

public class GitHubUser
{
    public string Login { get; set; }
    
    [JsonPropertyName("avatar_url")]
    public Uri AvatarUrl { get; set; }
}