using System;

namespace ChatBot.Core.Models
{
    public sealed class ClientMessage
    {
        public string ClientUserName { get; set; }

        public DateTime SendedOnUtc { get; set; }

        public string Message { get; set; }
    }
}
