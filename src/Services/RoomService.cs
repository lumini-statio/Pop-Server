using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ws_api.Models;
using System.Collections.Concurrent;

namespace ws_api.Services
{
    public class RoomService
    {
        private readonly ConcurrentDictionary<string, RoomUser> _byConnection = new();

        public void AddUser(string roomId, string userId, string connectionId)
        {
            var user = new RoomUser { RoomId = roomId, UserId = userId, ConnectionId = connectionId };
            _byConnection[connectionId] = user;
        }

        public RoomUser? RemoveByConnection(string connectionId)
        {
            _byConnection.TryRemove(connectionId, out var user);
            return user;
        }

        public string? GetConnectionId(string roomId, string userId)
        {
            return _byConnection.Values.FirstOrDefault(u => u.RoomId == roomId && u.UserId == userId)?.ConnectionId;
        }

        public IEnumerable<RoomUser> GetUsersInRoom(string roomId) =>
            _byConnection.Values.Where(u => u.RoomId == roomId);
    }
}