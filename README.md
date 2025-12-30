# PlaneMCP Server

This is the companion code for Season 08 Episode 01

## Configuration

Name | Description | Location
---|---|---
`PlaneAPIKey` | The API Key used to access the Plane app | User Secrets
`BaseUrl` | The base url for the Plane API | appsettings.json
`Workspace` | The Plane workspace | appsettings.json
`ProjectId` | The project id of the associated project | appsettings.json

## Set Up

1. Clone the repo:

```bash
git clone https://github.com/binarythistle/S08-E01-MCP-Server.git
```

2. Once cloned, change into the `S08-E01-MCP-Server` directory

3. Open the project (using VS Code this would be `code .`)

4. Open the `appsettings.json` file an update config elements to match your [Plane](https://plane.so/) Project details.

5. Obtain your Plane API key ([instructions here](https://developers.plane.so/api-reference/introduction#authentication)) and add the key as a user-secret as follows:

```bash
dotnet user-secrets set "PlaneAPIKey" "<your key>"
```

## Registering the Server

The server can now be registered with any MCP client app (e.g. VS Code with GitHub Co Pilot) to see it working using [MCP Inspector](https://modelcontextprotocol.io/docs/tools/inspector), first ensure that you have NodeJs installed (type `node --version` to find out if you have it), then type:

```bash
npx @modelcontextprotocol/inspector dotnet run
```

MCP Inspector should be loaded in a webpage, then:

- Click _Connect_
- Click _List Tools_
- Select `get_all_work_item_statuses`
- Click _Run Tool_

If everything has worked it should bring back a list of standard work items statuses from Plane.