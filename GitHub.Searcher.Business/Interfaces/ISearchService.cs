using GitHub.Searcher.Business.Entities;

namespace GitHub.Searcher.Business.Interfaces;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request);
}