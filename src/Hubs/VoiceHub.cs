using Microsoft.AspNetCore.SignalR;
using ws_api.Models;
using ws_api.Services;

namespace ws_api.Hubs
{
    public class VoiceHub : Hub
    {
        private readonly RoomService _roomService;

        public VoiceHub(RoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task JoinRoom(string roomId, string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            _roomService.AddUser(roomId, userId, Context.ConnectionId);

            await Clients.Group(roomId).SendAsync(
                "UserJoined",
                userId
            );
        }

        public async Task SendSignal(string roomId, SignalMessage message)
        {
            await Clients.OthersInGroup(roomId)
                .SendAsync("ReceiveSignal", message);
        }

        public async Task LeaveRoom(string roomId, string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            var user = _roomService.RemoveByConnection(Context.ConnectionId);
            if (user != null) await Clients.Group(roomId).SendAsync("UserLeft", userId);
        }

        public Task<IEnumerable<string>> GetUsersInRoom(string roomId)
        {
            return Task.FromResult(_roomService.GetUsersInRoom(roomId).Select(u => u.UserId));
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _roomService.RemoveByConnection(Context.ConnectionId);
            if (user != null)
            {
                await Clients.Group(user.RoomId)
                    .SendAsync("UserLeft", user.UserId);
            }
        }
    }
}