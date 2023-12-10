using EasyMicroservices.UI.Posts.Blazor.TestUI;
using EasyMicroservices.UI.Posts.ViewModels.Articles;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Post.GeneratedServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new ArticleClient("http://localhost:2010", sp.GetService<HttpClient>()));

builder.Services.AddScoped(sp => new FilterArticlesListViewModel(sp.GetService<ArticleClient>()));
builder.Services.AddScoped(sp => new AddOrUpdateArticleViewModel(sp.GetService<ArticleClient>()));
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});

await builder.Build().RunAsync();
