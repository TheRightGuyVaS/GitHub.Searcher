using Microsoft.AspNetCore.Mvc;
using GitHub.Searcher.Business.Entities;
using GitHub.Searcher.Business.Interfaces;

namespace Github.Searcher.Console.Controllers;

[Route("search")]
public class SearchController : Controller
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SearchResponse), 200)]
    public async Task<IActionResult> SearchAsync([FromQuery] SearchRequest request)
    {
        var response = await _searchService.SearchAsync(request);
        return Ok(response);
    }
}
