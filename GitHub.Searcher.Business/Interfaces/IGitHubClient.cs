using GitHub.Searcher.Business.Entities.GitHubResponse;

namespace GitHub.Searcher.Business.Interfaces;

public interface IGitHubClient
{
    Task<List<GitHubPullRequestResponse>> ListPullRequestsAsync(string owner, string repo, string label,
        IReadOnlyCollection<string> keywords);

    Task<List<GitHubCommitResponse>> GetCommitsForPullRequestAsync(string owner, string repo, int pullRequestNumber);
    Task<GitHubUserResponse?> GetUserByUsername(string username);

    Task<List<GitHubPullRequestCommentResponse>> GetCommentsForPullRequest(string owner, string repo,
        int pullRequestNumber);
}