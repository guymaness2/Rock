using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Utility;
using System.Threading.Tasks;

namespace Rock.Tests.Integration.Utility
{
    public static class AssertExtensions
    {
        public static void FileContains( this Assert assert, string filePath, string expectedString )
        {
            if ( string.IsNullOrWhiteSpace( filePath ) )
            {
                throw new ArgumentException( "No file path provided.", "filePath" );
            }

            if ( string.IsNullOrWhiteSpace( expectedString ) )
            {
                throw new ArgumentException( "No expected string provided.", "expectedString" );
            }

            var file = System.IO.File.ReadAllText( filePath );
            var fileContainsExpectedString = file.Contains( expectedString );

            if ( !fileContainsExpectedString )
            {
                throw new AssertFailedException( $"File {filePath} did not contain '{expectedString}'." );
            }

        }

        public static void FileDoesNotContains( this Assert assert, string filePath, string excludedString )
        {
            if ( string.IsNullOrWhiteSpace( filePath ) )
            {
                throw new ArgumentException( "No file path provided.", "filePath" );
            }

            if ( string.IsNullOrWhiteSpace( excludedString ) )
            {
                throw new ArgumentException( "No exclude string provided.", "excludedString" );
            }

            var file = System.IO.File.ReadAllText( filePath );
            var fileContainsExpectedString = file.Contains( excludedString );
            if ( fileContainsExpectedString )
            {
                throw new AssertFailedException( $"File {filePath} contain '{excludedString}'." );
            }
        }

        public static void FolderHasCorrectNumberOfFiles( this Assert assert, string folderPath, int fileCount )
        {
            if ( string.IsNullOrWhiteSpace( folderPath ) )
            {
                throw new ArgumentException( "No folder path provided.", "folderPath" );
            }

            var folder = System.IO.Directory.GetFiles( folderPath );
            var folderHasCorrectNumberOfFiles = folder.Length == fileCount;
            if ( !folderHasCorrectNumberOfFiles )
            {
                throw new AssertFailedException( $"The folder {folderPath} contain {folder.Length} files but expected {fileCount} files." );
            }
        }
    }

    /// <summary>
    /// Summary description for LoggerTests
    /// </summary>
    [TestClass]
    public class LoggerTests
    {
        private const string LOG_PATH = "logs\\rock.log";

        class RockLogConfiguration : IRockLogConfiguration
        {
            public RockLogLevel LogLevel { get; set; }
            public int MaxFileSize { get; set; }
            public int NumberOfLogFiles { get; set; }
            public List<string> DomainsToLog { get; set; }
            public string LogPath { get; set; }
        }

        [TestCleanup]
        public void Cleanup()
        {
            var logFolderPath = System.IO.Path.GetFullPath( System.IO.Path.GetDirectoryName( LOG_PATH ) );
            var files = System.IO.Directory.GetFiles( logFolderPath );
            foreach(var file in files )
            {
                System.IO.File.Delete( file );
            }
        }

        [TestMethod]
        public void LogInformationShouldOnlyLogCorrectDomains()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 1,
                DomainsToLog = new List<string> { "OTHER" },
                LogPath = LOG_PATH
            };
            var logger = new Logger( config );

            var expectedLogMessage = $"Test - {Guid.NewGuid()}";
            var excludedLogMessage = $"CRM Test - {Guid.NewGuid()}";

            logger.Information( expectedLogMessage );
            logger.Information( "CRM", excludedLogMessage );
            logger.Close();

            Assert.That.FileContains( config.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( config.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LogInformationShouldDomainsShouldBeCaseInsensitive()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 1,
                DomainsToLog = new List<string> { "OTHER", "crm" },
                LogPath = LOG_PATH
            };
            var logger = new Logger( config );

            var expectedLogMessage = $"Test - {Guid.NewGuid()}";
            var excludedLogMessage = $"CRM Test - {Guid.NewGuid()}";

            logger.Information( expectedLogMessage );
            logger.Information( "CRM", excludedLogMessage );
            logger.Close();

            Assert.That.FileContains( config.LogPath, expectedLogMessage );
            Assert.That.FileContains( config.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerShouldKeepOnlyMaxFileCount()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 2,
                DomainsToLog = new List<string> { "OTHER", "crm" },
                LogPath = LOG_PATH
            };
            var logger = new Logger( config );


            var maxByteCount = config.MaxFileSize * 1024 * 1024 * ( config.NumberOfLogFiles + 2 );
            var currentByteCount = 0;
            while ( currentByteCount < maxByteCount )
            {
                var expectedLogMessage = $"Test - {Guid.NewGuid()}";
                logger.Information( expectedLogMessage );

                currentByteCount += Encoding.ASCII.GetByteCount( expectedLogMessage );
            }

            logger.Close();
            var logFolderPath = System.IO.Path.GetFullPath( System.IO.Path.GetDirectoryName( config.LogPath ) );
            Assert.That.FolderHasCorrectNumberOfFiles( logFolderPath, config.NumberOfLogFiles );
        }

        [TestMethod]
        public void LoggerVerboseShouldLogDomainCorrectly()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 2,
                DomainsToLog = new List<string> { "OTHER", "crm" },
                LogPath = LOG_PATH
            };
            var logger = new Logger( config );

            var objectParams = new Object[] { config, RockLogLevel.All };
            var testException = new Exception( "Test Exception" );

            logger.Verbose( $"{Guid.NewGuid()}" );
            logger.Verbose( "CRM", $"{Guid.NewGuid()}" );

            logger.Verbose( $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );
            logger.Verbose( "CRM", $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );

            logger.Verbose( testException, $"{Guid.NewGuid()}" );
            logger.Verbose( "CRM", testException, $"{Guid.NewGuid()}" );

            logger.Verbose( testException, $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );
            logger.Verbose( "CRM", testException, $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );
        }

        [TestMethod]
        public void LoggerDebugShouldLogDomainCorrectly()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 2,
                DomainsToLog = new List<string> { "OTHER", "crm" },
                LogPath = LOG_PATH
            };
            var logger = new Logger( config );

            var objectParams = new Object[] { config, RockLogLevel.All };
            var testException = new Exception( "Test Exception" );

            logger.Debug( $"{Guid.NewGuid()}" );
            logger.Debug( "CRM", $"{Guid.NewGuid()}" );

            logger.Debug( $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );
            logger.Debug( "CRM", $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );

            logger.Debug( testException, $"{Guid.NewGuid()}" );
            logger.Debug( "CRM", testException, $"{Guid.NewGuid()}" );

            logger.Debug( testException, $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );
            logger.Debug( "CRM", testException, $"{Guid.NewGuid()} {{@oneProperty}} {{@twoProperty}}", config, RockLogLevel.All );
        }

        [TestMethod]
        public async Task LogInformationWorks()
        {
            Logger.Log.Information( "This is a test." );

            Logger.Log.Information( "coolPlugin", "This is a test." );

            var domainLogs = ProcessDomainLogs( 20, "This is a domain test" );
            var logs = ProcessLogs( 10, "This is a test" );

            await logs;
            await domainLogs;
        }

        private async Task ProcessDomainLogs( int logCount, string logMessage )
        {
            var taskList = new List<Task>();

            for ( var i = 0; i < logCount; i++ )
            {
                var index = i;
                taskList.Add( Task.Run( () => Logger.Log.Information( "coolPlugin", $"{logMessage} {index}." ) ) );
            }
            var t = Task.WhenAll( taskList.ToArray() );
            await t;
        }

        private async Task ProcessLogs( int logCount, string logMessage )
        {
            var taskList = new List<Task>();

            for ( var i = 0; i < logCount; i++ )
            {
                var index = i;
                taskList.Add( Task.Run( () =>
                {
                    Logger.Log.Information( $"{logMessage} {index}." );
                } ) );
            }
            var t = Task.WhenAll( taskList.ToArray() );
            await t;
        }
    }
}
