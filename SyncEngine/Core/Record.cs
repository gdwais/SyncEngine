using System;
using System.Collections.Generic;
using System.Text;

namespace SyncEngine.Core
{
    public class Record
    {
        public string DomainId { get; set; }
        public Dictionary<string, object> Data { get; set; }

    }
}
