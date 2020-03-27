using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

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
        string LogPath { get; set; }
    }

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

    public class Logger
    {
        private const string DEFAULT_DOMAIN = "OTHER";

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
                 .File( rockLogConfiguration.LogPath,
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
            if ( domain == null )
            {
                return false;
            }
            return rockLogConfiguration.DomainsToLog.Contains( domain.Value.ToString(), StringComparer.InvariantCultureIgnoreCase );
        }

        public void Close()
        {
            Serilog.Log.CloseAndFlush();
        }

        private string GetMessageTemplateWithDomain( string messageTemplate )
        {
            return $"{{domain}} {messageTemplate}";
        }

        private object[] AddDomainToObjectArray( object[] propertyValues, string domain )
        {
            var properties = new List<object>( propertyValues );
            properties.Insert( 0, domain );
            return properties.ToArray();
        }

        #region Verbose
        public void Verbose( string messageTemplate )
        {
            Verbose( DEFAULT_DOMAIN, messageTemplate );
        }

        public void Verbose( string domain, string messageTemplate )
        {
            Serilog.Log.Logger.Verbose( GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Verbose( string messageTemplate, params object[] propertyValues )
        {
            Verbose( DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void Verbose( string domain, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Verbose( GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }

        public void Verbose( Exception exception, string messageTemplate )
        {
            Verbose( DEFAULT_DOMAIN, exception, messageTemplate );
        }

        public void Verbose( string domain, Exception exception, string messageTemplate )
        {
            Serilog.Log.Logger.Verbose( exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Verbose( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Verbose( DEFAULT_DOMAIN, exception, messageTemplate, propertyValues );
        }

        public void Verbose( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Verbose( exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion

        #region Debug Methods

        public void Debug( string messageTemplate )
        {
            Debug( DEFAULT_DOMAIN, messageTemplate );
        }

        public void Debug( string domain, string messageTemplate )
        {
            Serilog.Log.Logger.Debug( GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Debug( string messageTemplate, params object[] propertyValues )
        {
            Debug( DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void Debug( string domain, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Debug( GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        
        public void Debug( Exception exception, string messageTemplate )
        {
            Debug( DEFAULT_DOMAIN, exception, messageTemplate );
        }

        public void Debug( string domain, Exception exception, string messageTemplate )
        {
            Serilog.Log.Logger.Debug( exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }
                
        public void Debug( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Debug( DEFAULT_DOMAIN, exception, messageTemplate, propertyValues );
        }

        public void Debug(string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Debug( exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion

        #region Information Methods
        public void Information<T>( string messageTemplate )
        {
            Information( DEFAULT_DOMAIN, messageTemplate );
        }

        public void Information( string messageTemplate, params object[] propertyValues )
        {
            Information( DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void Information( Exception exception, string messageTemplate )
        {
            Information( DEFAULT_DOMAIN, exception, messageTemplate );
        }

        public void Information( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Information( DEFAULT_DOMAIN, exception, messageTemplate, propertyValues );
        }

        public void Information<T>(string domain, string messageTemplate)
        {
            Serilog.Log.Logger.Information( GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Information( string domain, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Information( GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }

        public void Information( string domain, Exception exception, string messageTemplate )
        {
            Serilog.Log.Logger.Information( exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Information( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Information( exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion

        #region Warning Methods
        public void Warning( string messageTemplate )
        {
            Warning( DEFAULT_DOMAIN, messageTemplate );
        }

        public void Warning( string messageTemplate, params object[] propertyValues )
        {
            Warning( DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void Warning( Exception exception, string messageTemplate )
        {
            Warning( DEFAULT_DOMAIN, exception, messageTemplate );
        }

        public void Warning( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Warning( DEFAULT_DOMAIN, exception, messageTemplate, propertyValues );
        }

        public void Warning(string domain, string messageTemplate )
        {
            Serilog.Log.Logger.Warning( GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Warning( string domain, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Warning( GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }

        public void Warning( string domain, Exception exception, string messageTemplate )
        {
            Serilog.Log.Logger.Warning( exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Warning( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Warning( exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion

        #region Error Methods
        public void Error( string messageTemplate )
        {
            Error( DEFAULT_DOMAIN, messageTemplate );
        }

        public void Error( string messageTemplate, params object[] propertyValues )
        {
            Error( DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void Error( Exception exception, string messageTemplate )
        {
            Error( DEFAULT_DOMAIN, exception, messageTemplate );
        }

        public void Error( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Error( DEFAULT_DOMAIN, exception, messageTemplate, propertyValues );
        }

        public void Error(string domain, string messageTemplate )
        {
            Serilog.Log.Logger.Error( GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Error( string domain, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Error( GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }

        public void Error( string domain, Exception exception, string messageTemplate )
        {
            Serilog.Log.Logger.Error( exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Error( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Error( exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion

        #region Fatal Methods
        public void Fatal( string messageTemplate )
        {
            Fatal( DEFAULT_DOMAIN, messageTemplate );
        }

        public void Fatal( string messageTemplate, params object[] propertyValues )
        {
            Fatal( DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void Fatal( Exception exception, string messageTemplate )
        {
            Fatal( DEFAULT_DOMAIN, exception, messageTemplate );
        }

        public void Fatal( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Fatal( DEFAULT_DOMAIN, exception, messageTemplate, propertyValues );
        }
        public void Fatal(string domain, string messageTemplate )
        {
            Serilog.Log.Logger.Fatal( GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Fatal( string domain, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Fatal( GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }

        public void Fatal( string domain, Exception exception, string messageTemplate )
        {
            Serilog.Log.Logger.Fatal( exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void Fatal( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            Serilog.Log.Logger.Fatal( exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion
    }
}
