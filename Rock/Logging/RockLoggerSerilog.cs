using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Formatting.Compact;

namespace Rock.Logging
{
    internal class RockLoggerSerilog : IRockLogger
    {
        private const string DEFAULT_DOMAIN = "OTHER";
        private DateTime ConfigurationLastLoaded;
        private ILogger logger;

        public IRockLogConfiguration LogConfiguration { get; private set; }

        public RockLoggerSerilog( IRockLogConfiguration rockLogConfiguration )
        {
            LogConfiguration = rockLogConfiguration;
            LoadConfiguration( LogConfiguration );
        }

        ~RockLoggerSerilog()
        {
            if ( logger != null )
            {
                ( ( IDisposable ) logger ).Dispose();
                logger = null;
            }
        }

        public void Close()
        {
            if ( logger != null )
            {
                ( ( IDisposable ) logger ).Dispose();
                logger = null;
            }
        }

        #region WriteToLog Methods
        public void WriteToLog( RockLogLevel logLevel, string messageTemplate )
        {
            WriteToLog( logLevel, DEFAULT_DOMAIN, messageTemplate );
        }

        public void WriteToLog( RockLogLevel logLevel, string domain, string messageTemplate )
        {
            EnsureLoggerExistsAndUpdated();

            if ( LogConfiguration.LogLevel == RockLogLevel.Off || logLevel == RockLogLevel.Off )
            {
                return;
            }

            var serilogLogLevel = GetLogEventLevelFromRockLogLevel( logLevel );
            logger.Write( serilogLogLevel, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void WriteToLog( RockLogLevel logLevel, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( logLevel, DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void WriteToLog( RockLogLevel logLevel, string domain, string messageTemplate, params object[] propertyValues )
        {
            EnsureLoggerExistsAndUpdated();

            if ( LogConfiguration.LogLevel == RockLogLevel.Off || logLevel == RockLogLevel.Off )
            {
                return;
            }

            var serilogLogLevel = GetLogEventLevelFromRockLogLevel( logLevel );
            logger.Write( serilogLogLevel, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }

        public void WriteToLog( RockLogLevel logLevel, Exception exception, string messageTemplate )
        {
            WriteToLog( logLevel, exception, DEFAULT_DOMAIN, messageTemplate );
        }

        public void WriteToLog( RockLogLevel logLevel, Exception exception, string domain, string messageTemplate )
        {
            EnsureLoggerExistsAndUpdated();

            if ( LogConfiguration.LogLevel == RockLogLevel.Off || logLevel == RockLogLevel.Off )
            {
                return;
            }

            var serilogLogLevel = GetLogEventLevelFromRockLogLevel( logLevel );
            logger.Write( serilogLogLevel, exception, GetMessageTemplateWithDomain( messageTemplate ), domain.ToUpper() );
        }

        public void WriteToLog( RockLogLevel logLevel, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( logLevel, exception, DEFAULT_DOMAIN, messageTemplate, propertyValues );
        }

        public void WriteToLog( RockLogLevel logLevel, Exception exception, string domain, string messageTemplate, params object[] propertyValues )
        {
            EnsureLoggerExistsAndUpdated();

            if ( LogConfiguration.LogLevel == RockLogLevel.Off || logLevel == RockLogLevel.Off )
            {
                return;
            }

            var serilogLogLevel = GetLogEventLevelFromRockLogLevel( logLevel );
            logger.Write( serilogLogLevel, exception, GetMessageTemplateWithDomain( messageTemplate ), AddDomainToObjectArray( propertyValues, domain.ToUpper() ) );
        }
        #endregion

        #region Verbose Methods
        public void Verbose( string messageTemplate )
        {
            WriteToLog( RockLogLevel.All, messageTemplate );
        }

        public void Verbose( string domain, string messageTemplate )
        {
            WriteToLog( RockLogLevel.All, domain, messageTemplate );
        }

        public void Verbose( string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.All, messageTemplate, propertyValues );
        }

        public void Verbose( string domain, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.All, domain, messageTemplate, propertyValues );
        }

        public void Verbose( Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.All, exception, messageTemplate );
        }

        public void Verbose( string domain, Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.All, exception, domain, messageTemplate );
        }

        public void Verbose( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.All, exception, messageTemplate, propertyValues );
        }

        public void Verbose( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.All, exception, domain, messageTemplate, propertyValues );
        }
        #endregion

        #region Debug Methods

        public void Debug( string messageTemplate )
        {
            WriteToLog( RockLogLevel.Debug, messageTemplate );
        }

        public void Debug( string domain, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Debug, domain, messageTemplate );
        }

        public void Debug( string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Debug, messageTemplate, propertyValues );
        }

        public void Debug( string domain, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Debug, domain, messageTemplate, propertyValues );
        }

        public void Debug( Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Debug, exception, messageTemplate );
        }

        public void Debug( string domain, Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Debug, exception, domain, messageTemplate );
        }

        public void Debug( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Debug, exception, messageTemplate, propertyValues );
        }

        public void Debug( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Debug, exception, domain, messageTemplate, propertyValues );
        }

        #endregion

        #region Information Methods
        public void Information( string messageTemplate )
        {
            WriteToLog( RockLogLevel.Info, messageTemplate );
        }

        public void Information( string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Info, messageTemplate, propertyValues );
        }

        public void Information( Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Info, exception, messageTemplate );
        }

        public void Information( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Info, exception, messageTemplate, propertyValues );
        }

        public void Information( string domain, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Info, domain, messageTemplate );
        }

        public void Information( string domain, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Info, domain, messageTemplate, propertyValues );
        }

        public void Information( string domain, Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Info, exception, domain, messageTemplate );
        }

        public void Information( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Info, exception, domain, messageTemplate, propertyValues );
        }
        #endregion

        #region Warning Methods
        public void Warning( string messageTemplate )
        {
            WriteToLog( RockLogLevel.Warning, messageTemplate );
        }

        public void Warning( string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Warning, messageTemplate, propertyValues );
        }

        public void Warning( Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Warning, exception, messageTemplate );
        }

        public void Warning( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Warning, exception, messageTemplate, propertyValues );
        }

        public void Warning( string domain, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Warning, domain, messageTemplate );
        }

        public void Warning( string domain, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Warning, domain, messageTemplate, propertyValues );
        }

        public void Warning( string domain, Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Warning, exception, domain, messageTemplate );
        }

        public void Warning( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Warning, exception, domain, messageTemplate, propertyValues );
        }
        #endregion

        #region Error Methods
        public void Error( string messageTemplate )
        {
            WriteToLog( RockLogLevel.Error, messageTemplate );
        }

        public void Error( string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Error, messageTemplate, propertyValues );
        }

        public void Error( Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Error, exception, messageTemplate );
        }

        public void Error( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Error, exception, messageTemplate, propertyValues );
        }

        public void Error( string domain, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Error, domain, messageTemplate );
        }

        public void Error( string domain, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Error, domain, messageTemplate, propertyValues );
        }

        public void Error( string domain, Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Error, exception, domain, messageTemplate );
        }

        public void Error( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Error, exception, domain, messageTemplate, propertyValues );
        }
        #endregion

        #region Fatal Methods
        public void Fatal( string messageTemplate )
        {
            WriteToLog( RockLogLevel.Fatal, messageTemplate );
        }

        public void Fatal( string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Fatal, messageTemplate, propertyValues );
        }

        public void Fatal( Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Fatal, exception, messageTemplate );
        }

        public void Fatal( Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Fatal, exception, messageTemplate, propertyValues );
        }
        public void Fatal( string domain, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Fatal, domain, messageTemplate );
        }

        public void Fatal( string domain, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Fatal, domain, messageTemplate, propertyValues );
        }

        public void Fatal( string domain, Exception exception, string messageTemplate )
        {
            WriteToLog( RockLogLevel.Fatal, exception, domain, messageTemplate );
        }

        public void Fatal( string domain, Exception exception, string messageTemplate, params object[] propertyValues )
        {
            WriteToLog( RockLogLevel.Fatal, exception, domain, messageTemplate, propertyValues );
        }
        #endregion

        #region Private Helper Methods
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
            return LogConfiguration.DomainsToLog.Contains( domain.Value.ToString(), StringComparer.InvariantCultureIgnoreCase );
        }

        private void LoadConfiguration( IRockLogConfiguration rockLogConfiguration )
        {
            LogConfiguration.DomainsToLog.ForEach( s => s = s.ToUpper() );

            logger = new LoggerConfiguration()
                 .MinimumLevel
                 .Verbose()
                 .WriteTo
                 .File( new CompactJsonFormatter(),
                     rockLogConfiguration.LogPath,
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
            
            ConfigurationLastLoaded = DateTime.Now;
        }

        private void EnsureLoggerExistsAndUpdated()
        {
            if ( ConfigurationLastLoaded < LogConfiguration.LastUpdated || logger == null )
            {
                Close();
                LoadConfiguration( LogConfiguration );
            }
        }
        #endregion
    }
}
