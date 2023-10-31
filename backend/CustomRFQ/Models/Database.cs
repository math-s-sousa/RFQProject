using CustomRFQ.Utils;
using System.Reflection.Metadata.Ecma335;

namespace CustomRFQ.Models
{
    public class Database
    {
        public class EventSender
        {
            public string Guid { get; set; }
            public int DocEntry { get; set; }
            public int ObjType { get; set; }
            public string DB { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public char Status { get; set; }
        }

        public class Config
        {
            public string BaseUrl { get; set; }
            public string DB { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public SLApi SLApi { get; set; }
        }

        public class Smtp
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public bool SSL { get; set; }
            public string Host { get; set; }
            public string Password { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string BaseUrl { get; set; }
        }
    }
}
