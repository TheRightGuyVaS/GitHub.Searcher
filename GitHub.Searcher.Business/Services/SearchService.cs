using System.Runtime.CompilerServices;
using AutoMapper;
using GitHub.Searcher.Business.Entities;
using GitHub.Searcher.Business.Entities.GitHubResponse;
using GitHub.Searcher.Business.Interfaces;

[assembly: InternalsVisibleTo("GitHub.Searcher.Business.UnitTests")]
namespace GitHub.Searcher.Business.Services;

internal class SearchService : ISearchService
{
    private readonly IGitHubClient _gitHubClient;
    private readonly IMapper _mapper;

    public SearchService(IGitHubClient gitHubClient, IMapper mapper)
    {
        _gitHubClient = gitHubClient;
        _mapper = mapper;
    }
    
    public async Task<SearchResponse> SearchAsync(SearchRequest request)
    {
        //better to use fluent validation here
        if (string.IsNullOrEmpty(request.RepositoryOwner))
        {
            throw new ArgumentNullException(nameof(request.RepositoryOwner));
        }
        
        if (string.IsNullOrEmpty(request.RepositoryName))
        {
            throw new ArgumentNullException(nameof(request.RepositoryName));   
        }
        
        if (string.IsNullOrEmpty(request.PullRequestLabel))
        {
            throw new ArgumentNullException(nameof(request.PullRequestLabel));
        }
        
        if (request.Keywords == null || request.Keywords.Any() == false)
        {
            throw new ArgumentNullException(nameof(request.Keywords));
        }
        
        var gitHubListPullRequests = (await _gitHubClient.ListPullRequestsAsync(request.RepositoryOwner,
            request.RepositoryName, request.PullRequestLabel, request.Keywords)).AsEnumerable();
        var searchResponse = new SearchResponse();

        gitHubListPullRequests =
            gitHubListPullRequests.Where(x => x.State.Equals("open", StringComparison.OrdinalIgnoreCase));

        var loginToUser = new Dictionary<string, GitHubUserResponse>();
        var emailToUser = new Dictionary<string, GitHubUserResponse>();

        await Parallel.ForEachAsync(gitHubListPullRequests, async (pullRequest, _) =>
        {
            if (loginToUser.TryGetValue(pullRequest.User.Login, out var pullRequestAuthor) == false)
            {
                pullRequestAuthor = await _gitHubClient.GetUserByUsername(pullRequest.User.Login);
                loginToUser.Add(pullRequest.User.Login, pullRequestAuthor!);
                
                if (pullRequestAuthor != null && string.IsNullOrEmpty(pullRequestAuthor.Email) == false)
                {
                    emailToUser.Add(pullRequestAuthor.Email, pullRequestAuthor);
                }
            }

            var numberOfComments =
                (await _gitHubClient.GetCommentsForPullRequest(request.RepositoryOwner, request.RepositoryName,
                    pullRequest.Number)).Count;

            var gitHubGetCommitsForPullRequest =
                await _gitHubClient.GetCommitsForPullRequestAsync(request.RepositoryOwner, request.RepositoryName,
                    pullRequest.Number);
        
            var commitModels = new List<CommitModel>();
            foreach (var commit in gitHubGetCommitsForPullRequest)
            {
                var commitModel = _mapper.Map<CommitModel>(commit);
                
                commitModel.AuthorAvatarUrl = emailToUser.TryGetValue(commitModel.AuthorEmail, out var user)
                    ? user.AvatarUrl
                    : null;
                
                commitModels.Add(commitModel);
            }
            
            var pullRequestModel = _mapper.Map<PullRequestModel>(pullRequest);

            pullRequestModel.CreatorName = pullRequestAuthor?.Name;
            pullRequestModel.CreatorEmail = pullRequestAuthor?.Email;
            pullRequestModel.NumberOfComments = numberOfComments;
            pullRequestModel.Commits = commitModels;
        
            if (pullRequest.Draft)
            {
                lock (searchResponse.Draft)
                {
                    searchResponse.Draft.Add(pullRequestModel);                    
                }
                return;
            }
        
            if (DateTime.UtcNow.Subtract(pullRequestModel.CreatedAt).Days > 30)
            {
                lock (searchResponse.Stale)
                {
                    searchResponse.Stale.Add(pullRequestModel);
                }
                return;
            }

            lock (searchResponse.Active)
            {
                searchResponse.Active.Add(pullRequestModel);
            }
        });

        return searchResponse;
    }
}