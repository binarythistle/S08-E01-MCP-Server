
using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using PlaneMCPServer;

[McpServerToolType]
public class PlaneTools
{
    [McpServerTool, Description("Get a list of possible work item statuses. These statuses denote where in a typical kanban type workflow the work item is at. The status ID must be used with other tools, e.g. creating a work item")]
    public static async Task<string> GetAllWorkItemStatuses(PlaneApiService planeApiService)
    {
        var statuses = await planeApiService.GetProjectStatusesAsync();
        return JsonSerializer.Serialize(statuses);
    }
}