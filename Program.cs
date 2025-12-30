using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaneMCPServer;

// Build the host with dependency injection
var builder = Host.CreateApplicationBuilder(args);

// Ensure appsettings.json is loaded from the executable's directory
// This is critical for MCP servers launched by VS Code
builder.Configuration.Sources.Clear();
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddUserSecrets<Program>() // Add User Secrets support
    .AddEnvironmentVariables();

// Read configuration values with validation
var planeApiKey = builder.Configuration["PlaneAPIKey"];
var baseUrl = builder.Configuration["BaseUrl"];
var workspace = builder.Configuration["Workspace"];
var projectId = builder.Configuration["ProjectId"];

// Validate that all required configuration values are present
if (string.IsNullOrEmpty(planeApiKey))
    throw new InvalidOperationException("PlaneAPIKey is missing from configuration");
if (string.IsNullOrEmpty(baseUrl))
    throw new InvalidOperationException("BaseUrl is missing from configuration");
if (string.IsNullOrEmpty(workspace))
    throw new InvalidOperationException("Workspace is missing from configuration");
if (string.IsNullOrEmpty(projectId))
    throw new InvalidOperationException("ProjectId is missing from configuration");

// Register services
builder.Services.AddHttpClient(); // Register HttpClient factory
builder.Services.AddSingleton(sp => 
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new PlaneApiService(httpClientFactory, baseUrl, workspace, projectId, planeApiKey);
});


builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();