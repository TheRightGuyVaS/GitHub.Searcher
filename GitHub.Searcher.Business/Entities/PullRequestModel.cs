using System.Text.Json.Serialization;

namespace GitHub.Searcher.Business.Entities;

/// <summary>
/// Represents a model for a pull request
/// </summary>
public class PullRequestModel
{
    /// <summary>
    /// Number of the pull request
    /// </summary>
    [JsonIgnore]
    public int Number { get; set; }
    
    /// <summary>
    /// URL to the pull request page
    /// </summary>
    public Uri UrlToPage { get; set; }

    /// <summary>
    /// Title of the pull request
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Description of the pull request
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Number of comments on the pull request
    /// </summary>
    public int NumberOfComments { get; set; }

    /// <summary>
    /// Date and time when the pull request was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Name of the creator of the pull request
    /// </summary>
    public string? CreatorName { get; set; }

    /// <summary>
    /// Email of the creator of the pull request
    /// </summary>
    public string? CreatorEmail { get; set; }

    /// <summary>
    /// URL to the avatar of the creator of the pull request
    /// </summary>
    public Uri CreatorAvatarUrl { get; set; }

    /// <summary>
    /// Active pull request commits
    /// </summary>
    public IList<CommitModel> Commits { get; set; }

    public PullRequestModel()
    {
        Commits = new List<CommitModel>();
    }
}
