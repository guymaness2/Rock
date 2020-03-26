using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;

namespace Rock.Utility
{
    public enum RockLogLevel
    {
        Off,
        Fatal,
        Error,
        Warning,
        Info,
        Debug,
        All
    }

    public interface IRockLogConfiguration
    {
        RockLogLevel LogLevel { get; set; }
        int MaxFileSize { get; set; }
        int NumberOfLogFiles { get; set; }
        List<string> DomainsToLog { get; set; }
    }

    internal class RockLogConfiguration : IRockLogConfiguration
    {
        public RockLogLevel LogLevel { get; set; }
        public int MaxFileSize { get; set; }
        public int NumberOfLogFiles { get; set; }
        public List<string> DomainsToLog { get; set; }

        // TODO: Get from database
        //public RockLogConfiguration()
        //{

        //}


    }

    public class Logger
    {
        private IRockLogConfiguration rockLogConfiguration;
        private static Logger log;
        public static Logger Log
        {
            get
            {
                if ( log == null )
                {
                    // In the future the RockLogConfiguration could be gotten via dependency injection, but not today.
                    log = new Logger( new RockLogConfiguration() );
                }
                return log;
            }
        }

        public Logger( IRockLogConfiguration rockLogConfiguration )
        {
            this.rockLogConfiguration = rockLogConfiguration;

            this.rockLogConfiguration.DomainsToLog.ForEach( s => s = s.ToUpper() );

            Serilog.Log.Logger = new LoggerConfiguration()
                 .MinimumLevel
                 .Verbose()
                 .WriteTo
                 .File( "logs\\rock.log",
                     rollingInterval: RollingInterval.Infinite,
                     buffered: true,
                     shared: false,
                     restrictedToMinimumLevel: GetLogEventLevelFromRockLogLevel( rockLogConfiguration.LogLevel ),
                     retainedFileCountLimit: rockLogConfiguration.NumberOfLogFiles,
                     rollOnFileSizeLimit: true,
                     fileSizeLimitBytes: rockLogConfiguration.MaxFileSize * 1024 * 1024 )
                 .Filter
                 .ByIncludingOnly( ( e ) => ShouldLogDomain( e ) )
                 .CreateLogger();
        }

        ~Logger()
        {
            Serilog.Log.CloseAndFlush();
        }

        public void Information( string message )
        {
            Information( "Other", message );
        }

        public void Information( string domain, string message )
        {
            Serilog.Log.Logger.Information( "{domain} {message}", domain.ToUpper(), message );
        }

        private Serilog.Events.LogEventLevel GetLogEventLevelFromRockLogLevel( RockLogLevel logLevel )
        {
            switch ( logLevel )
            {
                case ( RockLogLevel.Error ):
                    return Serilog.Events.LogEventLevel.Error;
                case ( RockLogLevel.Warning ):
                    return Serilog.Events.LogEventLevel.Warning;
                case ( RockLogLevel.Info ):
                    return Serilog.Events.LogEventLevel.Information;
                case ( RockLogLevel.Debug ):
                    return Serilog.Events.LogEventLevel.Debug;
                case ( RockLogLevel.All ):
                    return Serilog.Events.LogEventLevel.Verbose;
                default:
                    return Serilog.Events.LogEventLevel.Fatal;
            }
        }

        private bool ShouldLogDomain( Serilog.Events.LogEvent e )
        {
            if ( !e.Properties.ContainsKey( "domain" ) )
            {
                return false;
            }
            var domain = e.Properties["domain"] as Serilog.Events.ScalarValue;
            if( domain == null )
            {
                return false;
            }
            return rockLogConfiguration.DomainsToLog.Contains( domain.Value );
        }
    }
}
