using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Persistence.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public User User { get; set; }
    }
}
