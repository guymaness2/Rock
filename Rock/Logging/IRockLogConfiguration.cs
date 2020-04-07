﻿using System;
using System.Collections.Generic;

namespace Rock.Logging
{
    /// <summary>
    /// Interface that is used be the RockLogger to store configuration data.
    /// </summary>
    public interface IRockLogConfiguration
    {
        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>
        /// The log level.
        /// </value>
        RockLogLevel LogLevel { get; set; }
        /// <summary>
        /// Gets or sets the maximum size of the file.
        /// </summary>
        /// <value>
        /// The maximum size of the file.
        /// </value>
        int MaxFileSize { get; set; }
        /// <summary>
        /// Gets or sets the number of log files.
        /// </summary>
        /// <value>
        /// The number of log files.
        /// </value>
        int NumberOfLogFiles { get; set; }
        /// <summary>
        /// Gets or sets the domains to log.
        /// </summary>
        /// <value>
        /// The domains to log.
        /// </value>
        List<string> DomainsToLog { get; set; }
        /// <summary>
        /// Gets or sets the log path.
        /// </summary>
        /// <value>
        /// The log path.
        /// </value>
        string LogPath { get; set; }
        /// <summary>
        /// Gets or sets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        DateTime LastUpdated { get; set; }
    }
}
