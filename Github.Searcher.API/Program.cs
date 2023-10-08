using GitHub.Searcher.Business;
using GitHub.Searcher.Business.Inftrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

BusinessDependencyInjectionModule.ConfigureServices(builder.Services, new GitHubClientSettings
{
    AccessToken = "place your token here"
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
