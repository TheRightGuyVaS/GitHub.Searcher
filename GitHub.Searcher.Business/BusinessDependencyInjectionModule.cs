using AutoMapper;
using GitHub.Searcher.Business.Inftrastructure;
using GitHub.Searcher.Business.Interfaces;
using GitHub.Searcher.Business.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("GitHub.Searcher.Business.UnitTests")]
namespace GitHub.Searcher.Business;

public static class BusinessDependencyInjectionModule
{
    public static void ConfigureServices(IServiceCollection serviceCollection, GitHubClientSettings settings)
    {
        serviceCollection.AddScoped<ISearchService, SearchService>();
        serviceCollection.AddScoped<IGitHubClient, GitHubClient>();
        serviceCollection.AddSingleton(settings);
        serviceCollection.AddSingleton(ConfigureMapper());
    }
    
    internal static IMapper ConfigureMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BusinessMapperProfile>();
        });

        return config.CreateMapper();
    }
}