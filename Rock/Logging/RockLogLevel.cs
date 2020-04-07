namespace Rock.Logging
{
    /// <summary>
    /// The Log Levels available for RockLogger.
    /// </summary>
    public enum RockLogLevel
    {
        /// <summary>
        /// Off - if this log level is specified nothing will be logged.
        /// </summary>
        Off,
        /// <summary>
        /// The fatal
        /// </summary>
        Fatal,
        /// <summary>
        /// The error
        /// </summary>
        Error,
        /// <summary>
        /// The warning
        /// </summary>
        Warning,
        /// <summary>
        /// The information
        /// </summary>
        Info,
        /// <summary>
        /// The debug
        /// </summary>
        Debug,
        /// <summary>
        /// All
        /// </summary>
        All
    }
}
