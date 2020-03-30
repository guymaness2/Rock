using System.Collections.Generic;
using Rock.Utility.Enums;
using Rock.Utility.Interfaces;

namespace Rock.Utility
{
    internal class RockLogConfiguration : IRockLogConfiguration
    {
        public RockLogLevel LogLevel { get; set; }
        public int MaxFileSize { get; set; }
        public int NumberOfLogFiles { get; set; }
        public List<string> DomainsToLog { get; set; }

        public string LogPath { get; set; }

        // TODO: Get from database
        //public RockLogConfiguration()
        //{

        //}
    }
}
