using Client.Components;
using Client.Constant;
using Client.Handlers;
using Client.Providers;
using Client.Services;
using Client.Services.Interfaces;
using Hanssens.Net;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings local   
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.Local.json",
        optional: true,
        reloadOnChange: true);
#endregion

#region Dependecies
builder.Services.AddTransient<JwtHandler>();

builder.Services.AddScoped<ILocalStorage, LocalStorage>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<IGenreService, GenreService>().AddHttpMessageHandler<JwtHandler>();
builder.Services.AddHttpClient<ITrackService, TrackService>().AddHttpMessageHandler<JwtHandler>();
builder.Services.AddHttpClient<IMovieService, MovieService>().AddHttpMessageHandler<JwtHandler>();
builder.Services.AddHttpClient<IUserInteractionService, UserInteractionService>().AddHttpMessageHandler<JwtHandler>();
builder.Services.AddHttpClient<IMediaService, MediaService>().AddHttpMessageHandler<JwtHandler>();
builder.Services.AddHttpClient<IRecommendationService, RecommendationService>().AddHttpMessageHandler<JwtHandler>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped(sp => (CustomAuthStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
#endregion


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthentication("JwtAuth")
    .AddCookie("JwtAuth", options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/not-found";
    });

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous();

app.Run();
