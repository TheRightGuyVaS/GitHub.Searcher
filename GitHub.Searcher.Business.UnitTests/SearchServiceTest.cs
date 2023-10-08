using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using GitHub.Searcher.Business.Entities;
using GitHub.Searcher.Business.Entities.GitHubResponse;
using GitHub.Searcher.Business.Interfaces;
using GitHub.Searcher.Business.Services;
using Xunit;

namespace GitHub.Searcher.Business.UnitTests;

public class SearchServiceTest
{
    private const string RepoOwner = "repoOwner";
    private const string RepoName = "repoName";
    private const string PullRequestLabel = "pullRequestLabel";
    private const int PullRequestNumber = 1;
    private const string PullRequestTitle = "pullRequestTitle";
    private static readonly IReadOnlyCollection<string> Keywords = new List<string> {PullRequestTitle, "keyword2"};

    private readonly IGitHubClient _gitHubClient;
    private readonly ISearchService _searchService;

    public SearchServiceTest()
    {
        var mapper = BusinessDependencyInjectionModule.ConfigureMapper();
        _gitHubClient = Substitute.For<IGitHubClient>();
        _searchService = new SearchService(_gitHubClient, mapper);
    }

    [Theory]
    [InlineData(false, 0, PullRequestCategory.Active)]
    [InlineData(false, -29, PullRequestCategory.Active)]
    [InlineData(false, -30, PullRequestCategory.Active)]
    [InlineData(false, -31, PullRequestCategory.Stale)]
    [InlineData(false, -100, PullRequestCategory.Stale)]
    [InlineData(true, 0, PullRequestCategory.Draft)]
    [InlineData(true, -29, PullRequestCategory.Draft)]
    [InlineData(true, -30, PullRequestCategory.Draft)]
    [InlineData(true, -100, PullRequestCategory.Draft)]
    public async Task should_put_pullRequest_in_proper_collection(bool isDraft, int createdAtDiffInDaysFromToday,
        PullRequestCategory expected)
    {
        //arrange
        var pullRequest = new GitHubPullRequestResponse
        {
            Number = PullRequestNumber,
            Title = PullRequestTitle,
            CreatedAt = DateTime.UtcNow.AddDays(createdAtDiffInDaysFromToday),
            Draft = isDraft,
            State = "open",
            User = new GitHubUser
            {
                Login = "login"
            }
        };
        
        _gitHubClient.ListPullRequestsAsync(RepoOwner, RepoName, PullRequestLabel, Keywords)
            .Returns(new List<GitHubPullRequestResponse> {pullRequest});
        _gitHubClient.GetCommentsForPullRequest(RepoOwner, RepoName, PullRequestNumber)
            .Returns(new List<GitHubPullRequestCommentResponse>());
        _gitHubClient.GetCommitsForPullRequestAsync(RepoOwner, RepoName, PullRequestNumber)
            .Returns(new List<GitHubCommitResponse>());
        
        //act
        var actualResponse = await _searchService.SearchAsync(new SearchRequest
        {
            RepositoryOwner = RepoOwner,
            RepositoryName = RepoName,
            PullRequestLabel = PullRequestLabel,
            Keywords = Keywords
        });

        var actualCategory = actualResponse.Active.Count > 0 
            ? PullRequestCategory.Active 
            : actualResponse.Draft.Count > 0 
                ? PullRequestCategory.Draft 
                : PullRequestCategory.Stale;

        //assert
        actualCategory.Should().Be(expected);
    }

    public enum PullRequestCategory
    {
        Active,
        Draft,
        Stale
    }
}