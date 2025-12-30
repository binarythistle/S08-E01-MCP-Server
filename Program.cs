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

// Validate that all required configuration values are present
if (string.IsNullOrEmpty(planeApiKey))
{
    Console.WriteLine("Error: PlaneAPIKey is not configured in user secrets.");
    return;
}

if (string.IsNullOrEmpty(baseUrl))
{
    Console.WriteLine("Error: BaseUrl is not configured in appsettings.json.");
    return;
}

if (string.IsNullOrEmpty(workspace))
{
    Console.WriteLine("Error: Workspace is not configured in appsettings.json.");
    return;
}

if (string.IsNullOrEmpty(projectId))
{
    Console.WriteLine("Error: ProjectId is not configured in appsettings.json.");
    return;
}

// Register services
builder.Services.AddHttpClient(); // Register HttpClient factory
builder.Services.AddSingleton(sp => 
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new PlaneApiService(httpClientFactory, baseUrl!, workspace!, projectId!, planeApiKey!);
});

var host = builder.Build();

// Display configuration (masked API key for security)
Console.WriteLine("Configuration loaded successfully:");
Console.WriteLine($"BaseUrl: {baseUrl}");
Console.WriteLine($"Workspace: {workspace}");
Console.WriteLine($"ProjectId: {projectId}");
Console.WriteLine($"PlaneAPIKey: {new string('*', planeApiKey!.Length)} (masked)");
Console.WriteLine();

// Get the API service from DI container
var planeService = host.Services.GetRequiredService<PlaneApiService>();

// Test API calls
Console.WriteLine("=== Testing Plane API Calls ===\n");

// 1. Get Project Statuses
Console.WriteLine("1. Get Project Statuses...");
try
{
    var statusesJson = await planeService.GetProjectStatusesAsync();
    Console.WriteLine("✓ Success\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}\n");
}

// 2. Get All Work Items
Console.WriteLine("2. Get All Work Items...");
try
{
    var workItemsJson = await planeService.GetAllWorkItemsAsync();
    Console.WriteLine("✓ Success\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}\n");
}

// 3. Search Work Items
Console.WriteLine("3. Search Work Items (searching for 'tooer')...");
try
{
    var searchJson = await planeService.SearchWorkItemsAsync("tooer");
    Console.WriteLine("✓ Success\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}\n");
}

// 4. Create Work Item
Console.WriteLine("4. Create Work Item...");
try
{
    // First get a valid state ID from the statuses
    var statusesJson = await planeService.GetProjectStatusesAsync();
    var statusesDoc = System.Text.Json.JsonDocument.Parse(statusesJson);
    var firstStateId = statusesDoc.RootElement.GetProperty("results")[0].GetProperty("id").GetString();
    
    var createJson = await planeService.CreateWorkItemAsync(
        "Test Work Item from API",
        "<p>This work item was created via the PlaneApiService</p>",
        firstStateId!
    );
    Console.WriteLine("✓ Success - Work item created\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}\n");
}

Console.WriteLine("=== All API tests completed ===");
