using System;
using System.Collections.Generic;

namespace Rock.Logging
{
    public interface IRockLogConfiguration
    {
        RockLogLevel LogLevel { get; set; }
        int MaxFileSize { get; set; }
        int NumberOfLogFiles { get; set; }
        List<string> DomainsToLog { get; set; }
        string LogPath { get; set; }
        DateTime LastUpdated { get; set; }
    }
}
