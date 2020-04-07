using System.Collections.Generic;

namespace Rock.Logging
{
    /// <summary>
    /// This is a simple POCO used to store the System Settings for Rock Logging.
    /// </summary>
    public class RockLogSystemSettings
    {
        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        public RockLogLevel LogLevel { get; set; }
        /// <summary>
        /// Gets or sets the maximum size of the file.
        /// </summary>
        /// <value>
        /// The maximum size of the file.
        /// </value>
        public int MaxFileSize { get; set; }
        /// <summary>
        /// Gets or sets the number of log files.
        /// </summary>
        /// <value>
        /// The number of log files.
        /// </value>
        public int NumberOfLogFiles { get; set; }
        /// <summary>
        /// Gets or sets the domains to log.
        /// </summary>
        /// <value>
        /// The domains to log.
        /// </value>
        public List<string> DomainsToLog { get; set; }
    }
}
