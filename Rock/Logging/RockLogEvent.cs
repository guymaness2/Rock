using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Logging
{
    public class RockLogEvent
    {
        public string Domain { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public Exception Exception { get; set; }
        public RockLogLevel Level { get; set; }
    }
}
