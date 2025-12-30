using Microsoft.Extensions.Configuration;

// Build configuration from appsettings.json and user secrets
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

// Read configuration values
var planeApiKey = configuration["PlaneAPIKey"];
var baseUrl = configuration["BaseUrl"];
var workspace = configuration["Workspace"];
var projectId = configuration["ProjectId"];


