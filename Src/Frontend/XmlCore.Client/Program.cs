

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;


using Blazorise.Bootstrap5.Components;
using XmlCore.Client.Authentication;
using XmlCore.Client.Components;
using XmlCore.Client.Components.Pages;
using XmlCore.Client.Services;
using XmlCore.Shared.DTO;
using XmlCore.Shared.Entities;
using XmlCore.Shared.Interface;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("XmlCoreAPI",
    client =>
    {
        client.BaseAddress = new Uri("https://localhost:7127");
    });


#region Services
builder.Services.AddScoped<IUserInterface<User>, UserService>();
builder.Services.AddScoped<ICategoryInterface<Category>, CategoryService>();
builder.Services.AddScoped<ICompanyInterface<CompanyDTO>, CompanyService>();
builder.Services.AddScoped<IOrderInterface<OrderDTO>, OrderService>();
builder.Services.AddScoped<IArticleInterface<ArticleDTO>, ArticleService>();
builder.Services.AddScoped<IArticlesInOrderInterface<ArticlesInOrderDTO>, ArticleInOrderService>();

//Company/User redirection
builder.Services.AddSingleton<CompanyManager>();
builder.Services.AddSingleton<UserManager>();

builder.Services.AddSingleton<Shop.Cart>();


#endregion



//Detta är för att Authentication ska fungera
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthenticationService>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
