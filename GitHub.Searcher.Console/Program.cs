// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using GitHub.Searcher.Console.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using GitHub.Searcher.Business;
using GitHub.Searcher.Business.Entities;
using GitHub.Searcher.Business.Inftrastructure;
using GitHub.Searcher.Business.Interfaces;

ConsoleHelper.ClearConsole();
"Application started".Display();
ConsoleHelper.HorizontalLine();

var sw = new Stopwatch();
sw.Start();

try
{
    var sc = new ServiceCollection();
    BusinessDependencyInjectionModule.ConfigureServices(sc, new GitHubClientSettings
    {
        AccessToken = "place your token here"
    });
    var sp = sc.BuildServiceProvider();
    
    var searchService = sp.GetRequiredService<ISearchService>();
}
catch (Exception e)
{
    e.Display();
}

sw.Stop();
ConsoleHelper.HorizontalLine();
$"Application finished in {sw.ElapsedMilliseconds} ms".Display();

