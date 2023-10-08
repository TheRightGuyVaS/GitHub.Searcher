using System.Text.Json.Serialization;

namespace GitHub.Searcher.Business.Entities.GitHubResponse;

public class GitHubUserResponse
{
    [JsonPropertyName("avatar_url")]
    public Uri AvatarUrl { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}