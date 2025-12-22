using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ws_api.Services
{
    public class ChatWebSocketHandler
    {
        public ChatWebSocketHandler()
        {
            
        }
        public async Task HandleAsync(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var response = Encoding.UTF8.GetBytes("ok");

                    await socket.SendAsync(
                        response,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                        );
                }
            }
        }
    }

}