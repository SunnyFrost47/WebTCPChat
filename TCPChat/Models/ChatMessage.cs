using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCPChat.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
    }
}