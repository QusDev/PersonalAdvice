using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Data.DbContext;
using Server.Data.Entities.Identity;
using Server.Data.Seeders;
using Server.Mapping;
using Server.Services.Entities;
using Server.Services.Entities.Interfaces;
using Server.Services.Identity;
using Server.Services.Identity.Interfaces;
using Server.Services.Jamendo;
using Server.Services.Jamendo.Interfaces;
using Server.Services.Recommendation;
using Server.Services.Recommendation.Interfaces;
using Server.Services.Tmdb;
using Server.Services.Tmdb.Interfaces;
using Server.UnitOfWork;
using Shared.Constants;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings local   
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.Local.json",
        optional: true,
        reloadOnChange: true);
#endregion

#region DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException($"Connection string {"DefaultConnection"} is missing in appsettings.json or environment variables.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

//builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
//     options.UseNpgsql(connectionString));
#endregion

#region Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(opt =>
    {
        opt.Password.RequireDigit = true;
        opt.Password.RequiredLength = 6;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireUppercase = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequiredUniqueChars = 1;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

#region Roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(Role.Admin));
    options.AddPolicy("UserOnly", policy => policy.RequireRole(Role.User));
});
#endregion

#region Dependencies
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMediaCollaboratorService, MediaCollaboratorService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IUserInteractionService, UserInteractionService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IJamendoService, JamendoService>();
builder.Services.AddScoped<ITmdbService, TmdbService>();

builder.Services.AddScoped<IRecommendationService, RecommendationService>();

builder.Services.AddHttpClient<ITmdbService, TmdbService>();
builder.Services.AddHttpClient<IJamendoService, JamendoService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(EntitiesMappingProfile).Assembly);
#endregion

#region NSwag
builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Movie & Music Recommendation API";
    document.Version = "v1";

    document.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = NSwag.OpenApiSecurityApiKeyLocation.Header,
        Description = "Введіть токен у форматі: Bearer {ваш_токен}"
    });

    // Цей процесор автоматично додає іконку "замка" до методів з атрибутом [Authorize]
    document.OperationProcessors.Add(
        new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
});
#endregion

#region Cors
var allowedOrigins = builder.Configuration.GetSection("BlazorAllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
    {
        if (allowedOrigins != null && allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    });
});
#endregion

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

#region Seeders
using (var scope = app.Services.CreateScope())
{
    await RoleSeeder.SeedRolesAsync(scope.ServiceProvider);
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseOpenApi();

    app.UseSwaggerUi();
}

app.UseCors("BlazorPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
