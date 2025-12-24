using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ws_api.Models
{
    public class RoomUser
    {
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
    }
}