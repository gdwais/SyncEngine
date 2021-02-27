using System;

namespace SyncEngine.Core.Configuration
{
    public class MessagesSettings
    {
        public string Host { get; set; } = String.Empty;
        public int Port { get; set; } = 0;
        public string UserName { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string RouteKey { get; set; } = String.Empty;
    }
}