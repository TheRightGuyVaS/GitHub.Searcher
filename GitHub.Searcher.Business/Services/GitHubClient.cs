using System.Net.Http.Json;
using GitHub.Searcher.Business.Entities.GitHubResponse;
using GitHub.Searcher.Business.Inftrastructure;
using GitHub.Searcher.Business.Interfaces;

namespace GitHub.Searcher.Business.Services;

internal class GitHubClient : IGitHubClient
{
    private const string BaseUrl = "https://api.github.com/";

    private readonly HttpClient _httpClient;

    public GitHubClient(GitHubClientSettings settings)
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
        _httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + settings.AccessToken);
    }

    public async Task<List<GitHubPullRequestResponse>> ListPullRequestsAsync(string owner, string repo, string label,
        IReadOnlyCollection<string> keywords)
    {
        var allPullRequests = new List<GitHubPullRequestResponse>();
        var page = 1;

        while (true)
        {
            var url = $"{BaseUrl}repos/{owner}/{repo}/pulls?state=all&labels={label}&per_page=100&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var pullRequests = await response.Content.ReadFromJsonAsync<List<GitHubPullRequestResponse>>() ??
                                   new List<GitHubPullRequestResponse>();

                if (pullRequests.Count == 0)
                {
                    break;
                }

                foreach (var pr in pullRequests)
                {
                    if (keywords.Count > 0)
                    {
                        var containsKeyword = false;
                        foreach (var keyword in keywords)
                        {
                            if (pr.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                            {
                                containsKeyword = true;
                                break;
                            }
                        }

                        if (!containsKeyword)
                            continue;
                    }

                    allPullRequests.Add(pr);
                }

                page++;
            }
            else
            {
                throw new Exception($"Error fetching pull requests: {response.ReasonPhrase}");
            }
        }

        return allPullRequests;
    }

    public async Task<List<GitHubCommitResponse>> GetCommitsForPullRequestAsync(string owner, string repo,
        int pullRequestNumber)
    {
        var allCommits = new List<GitHubCommitResponse>();
        var page = 1;

        while (true)
        {
            var url = $"{BaseUrl}repos/{owner}/{repo}/pulls/{pullRequestNumber}/commits?per_page=100&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var commits = await response.Content.ReadFromJsonAsync<List<GitHubCommitResponse>>() ??
                              new List<GitHubCommitResponse>();

                if (commits.Count == 0)
                {
                    break;
                }

                allCommits.AddRange(commits);
                page++;
            }
            else
            {
                throw new Exception($"Error fetching commits: {response.ReasonPhrase}");
            }
        }

        return allCommits;
    }

    public async Task<GitHubUserResponse?> GetUserByUsername(string username)
    {
        var url = $"{BaseUrl}users/{username}";

        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<GitHubUserResponse>();
        }

        throw new Exception($"Error fetching user: {response.ReasonPhrase}");
    }

    public async Task<List<GitHubPullRequestCommentResponse>> GetCommentsForPullRequest(string owner, string repo,
        int pullRequestNumber)
    {
        var page = 1;

        var allComments = new List<GitHubPullRequestCommentResponse>();

        while (true)
        {
            var url = $"{BaseUrl}repos/{owner}/{repo}/issues/{pullRequestNumber}/comments?per_page=100&page={page}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var comments = await response.Content.ReadFromJsonAsync<List<GitHubPullRequestCommentResponse>>() ??
                               new List<GitHubPullRequestCommentResponse>();

                if (comments.Count == 0)
                {
                    break;
                }

                allComments.AddRange(comments);
                page++;
            }
            else
            {
                throw new Exception($"Error fetching comments: {response.ReasonPhrase}");
            }
        }

        return allComments;
    }
}