using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaneMCPServer;

// Build the host with dependency injection
var builder = Host.CreateApplicationBuilder(args);

// Add user secrets to configuration
builder.Configuration.AddUserSecrets<Program>();

// Read configuration values
var planeApiKey = builder.Configuration["PlaneAPIKey"];
var baseUrl = builder.Configuration["BaseUrl"];
var workspace = builder.Configuration["Workspace"];
var projectId = builder.Configuration["ProjectId"];


// Register services
builder.Services.AddHttpClient(); // Register HttpClient factory
builder.Services.AddSingleton(sp => 
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new PlaneApiService(httpClientFactory, baseUrl!, workspace!, projectId!, planeApiKey!);
});


builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();