using System.Collections.Generic;
using System.Threading;
using Rock.Logging;
using Rock.SystemKey;

namespace Rock.Tests.Integration
{
    public static class RockLoggingHelpers
    {
        public static void SaveRockLogConfiguration( List<string> domainsToLog = null, RockLogLevel logLevel = RockLogLevel.Off, int maxFileSize = 0, int maxFiles = 0 )
        {
            if ( domainsToLog == null )
            {
                domainsToLog = new List<string> { RockLogDomains.Other };
            }

            var logConfig = new RockLogSystemSettings
            {
                LogLevel = logLevel,
                DomainsToLog = domainsToLog,
                MaxFileSize = maxFileSize,
                NumberOfLogFiles = maxFiles
            };

            Rock.Web.SystemSettings.SetValue( SystemSetting.ROCK_LOGGING_SETTINGS, logConfig.ToJson() );
        }

        public static void DeleteFilesInFolder( string logFolder )
        {
            var logFolderPath = System.IO.Path.GetFullPath( logFolder );
            if ( !System.IO.Directory.Exists( logFolderPath ) )
            {
                return;
            }

            var files = System.IO.Directory.GetFiles( logFolderPath, "*.*", System.IO.SearchOption.AllDirectories );
            var retryCount = 2;
            foreach ( var file in files )
            {
                for ( int i = 0; i < retryCount; i++ )
                {
                    try
                    {
                        System.IO.File.Delete( file );
                        break;
                    }
                    catch
                    {
                        Thread.SpinWait( 1000 );
                    }
                }
            }

            System.IO.Directory.Delete( logFolder, true );
        }
    }
}
