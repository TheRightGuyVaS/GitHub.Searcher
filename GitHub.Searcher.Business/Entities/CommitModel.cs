namespace GitHub.Searcher.Business.Entities;

/// <summary>
/// Represents a commit in a repository.
/// </summary>
public class CommitModel
{
    /// <summary>
    /// Unique hash of the commit.
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    /// Name of the author of the commit.
    /// </summary>
    public string AuthorName { get; set; }

    /// <summary>
    /// Email of the author of the commit.
    /// </summary>
    public string AuthorEmail { get; set; }

    /// <summary>
    /// URL to the avatar of the commit author.
    /// </summary>
    public Uri? AuthorAvatarUrl { get; set; }

    /// <summary>
    /// Date and time when the commit was made.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Message associated with the commit.
    /// </summary>
    public string Message { get; set; }
}
