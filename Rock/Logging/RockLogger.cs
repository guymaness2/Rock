using System.Web;

namespace Rock.Logging
{
    public static class RockLogger
    {
        private static IRockLogger log;
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
    }
}
