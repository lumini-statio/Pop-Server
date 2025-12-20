using Fleck;

var server = new WebSocketServer("ws://0.0.0.0:8181");

var wsConnections = new List<IWebSocketConnection>();

server.Start(ws =>
{
    ws.OnOpen = () =>
    {
        wsConnections.Add(ws);
    };

    ws.OnMessage = message =>
    {
        foreach (var conn in wsConnections)
        {
            conn.Send(message);
        }
    };
});

WebApplication.CreateBuilder(args).Build().Run();
