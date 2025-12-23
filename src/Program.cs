using Fleck;
using Microsoft.EntityFrameworkCore;
using ws_api.Database;
using ws_api.Services;

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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ChatWebSocketHandler>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseWebSockets();

app.MapControllers();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var manager = context.RequestServices.GetRequiredService<ChatWebSocketHandler>();
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        await manager.HandleAsync(socket);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();
