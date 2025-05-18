using CV_Site.CachServices;
using Service;
using Services;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddUserSecrets<Program>();


builder.Services.Configure<GitHubIntegrationOptions>(
    builder.Configuration.GetSection("GitHubIntegrationOptions"));



builder.Services.AddMemoryCache();

builder.Services.AddScoped<IGitHubService,GitHubService>();
builder.Services.Decorate<IGitHubService,CachedGitHubService>();

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
