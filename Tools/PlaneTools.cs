
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

    [McpServerTool, Description("Get all work items. This returns all work items irrespective of status")]
    public static async Task<string> GetAllWorkItems(PlaneApiService planeApiService)
    {
        var workItems = await planeApiService.GetAllWorkItemsAsync();
        return JsonSerializer.Serialize(workItems);
    }

    [McpServerTool, Description("This tool allows you to create a simple work item. It does require a status Id which can be derived from the GetAllWorkItemStatuses Tool. Default should be Backlog if not supplied.")]
    public static async Task<string> CreateWorkItem(PlaneApiService planeApiService, [Description("The main title for the work item")] string name, [Description("The high level description for the work item")] string description, [Description("The status ID of the work item , derived from the GetAllWorkItemStatues tool")] string statusId)
    {
        var workItem = await planeApiService.CreateWorkItemAsync(name, description, statusId);
        return JsonSerializer.Serialize(workItem);
    }
}