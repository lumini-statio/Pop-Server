using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ws_api.Models
{
    public class SignalMessage
    {
        public string Type { get; set; }   // "offer" | "answer" | "ice"
        public string From { get; set; }
        public string To { get; set; }
        public string Payload { get; set; } // SDP o ICE JSON
    }
}