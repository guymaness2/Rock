using System.Collections.Generic;
using Rock.Utility.Enums;

namespace Rock.Utility.Interfaces
{
    public interface IRockLogConfiguration
    {
        RockLogLevel LogLevel { get; set; }
        int MaxFileSize { get; set; }
        int NumberOfLogFiles { get; set; }
        List<string> DomainsToLog { get; set; }
        string LogPath { get; set; }
    }
}
