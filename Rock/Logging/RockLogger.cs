namespace Rock.Logging
{
    /// <summary>
    /// This is the static class that is used to log data in Rock.
    /// </summary>
    public static class RockLogger
    {
        private static IRockLogger log;
        /// <summary>
        /// Gets the logger with logging methods.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public static IRockLogger Log
        {
            get
            {
                if ( log == null )
                {
                    // In the future the RockLogConfiguration could be gotten via dependency injection, but not today.
                    log = new RockLoggerSerilog( new RockLogConfiguration() );
                }
                return log;
            }
        }

        /// <summary>
        /// Gets the log reader.
        /// </summary>
        /// <value>
        /// The log reader.
        /// </value>
        public static IRockLogReader LogReader => new RockSerilogReader( Log );

    }
}
