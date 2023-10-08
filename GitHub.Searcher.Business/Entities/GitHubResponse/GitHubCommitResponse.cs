namespace GitHub.Searcher.Business.Entities.GitHubResponse;

public class GitHubCommitResponse
{
    public string Sha { get; set; }
    public Commit Commit { get; set; }
}

public class Commit
{
    public Author Author { get; set; }
    public string Message { get; set; }
}

public class Author
{
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
}