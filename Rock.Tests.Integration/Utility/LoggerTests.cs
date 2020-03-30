using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Utility;
using Rock.Utility.Enums;
using Rock.Utility.Interfaces;

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
                throw new AssertFailedException( $"File {filePath} contained '{excludedString}'." );
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

        public static void FileNotFound( this Assert assert, string filePath )
        {
            if ( string.IsNullOrWhiteSpace( filePath ) )
            {
                throw new ArgumentException( "No file path provided.", "filePath" );
            }

            if ( System.IO.File.Exists( filePath ) )
            {
                throw new AssertFailedException( $"File {filePath} was found." );
            }

        }

        public static void FolderFileSizeIsWithinRange( this Assert assert, string folderPath, long minFileSize, long maxFileSize, double allowedVariation )
        {
            if ( string.IsNullOrWhiteSpace( folderPath ) )
            {
                throw new ArgumentException( "No folder path provided.", "folderPath" );
            }

            var folder = System.IO.Directory.GetFiles( folderPath );
            var allowedMinFileSize = minFileSize - ( minFileSize * allowedVariation );
            var allowedMaxFileSize = maxFileSize + ( maxFileSize * allowedVariation );

            foreach ( var filePath in folder )
            {
                var file = System.IO.File.OpenRead( filePath );
                var fileSize = file.Length;
                if ( fileSize > allowedMaxFileSize || fileSize < allowedMinFileSize )
                {
                    throw new AssertFailedException( $"The {filePath} file size is {fileSize}, but was expected to be between {minFileSize} and {maxFileSize}." );
                }
                file.Close();
            }
        }
    }

    [TestClass]
    public class LoggerTests
    {
        private const string LOG_FOLDER = "\\logs";
        private const string TEST_LOGGER_CONFIG_SERIALIZATION = "{\"LogLevel\":\"All\",\"MaxFileSize\":1,\"NumberOfLogFiles\":2,\"DomainsToLog\":[\"OTHER\",\"crm\"],\"LogPath\":\"{logPath}\",\"$type\":\"RockLogConfiguration\"}";
        private const string TEST_EXCEPTION_SERIALIZATION = "\r\nSystem.Exception: Test Exception";

        private readonly Exception TestException = new Exception( "Test Exception" );

        [TestCleanup]
        public void Cleanup()
        {
            DeleteFilesInFolder( LOG_FOLDER );
        }

        [TestMethod]
        public void LoggerShouldKeepOnlyMaxFileCount()
        {
            var logger = GetTestLogger( logFolder: "MaxFileTest" );

            CreateLogFiles( logger, logger.LogConfiguration.MaxFileSize, logger.LogConfiguration.NumberOfLogFiles + 2 );

            logger.Close();

            var logFolderPath = System.IO.Path.GetFullPath( System.IO.Path.GetDirectoryName( logger.LogConfiguration.LogPath ) );

            Assert.That.FolderHasCorrectNumberOfFiles( logFolderPath, logger.LogConfiguration.NumberOfLogFiles );

            DeleteFilesInFolder( logFolderPath );
        }

        [TestMethod]
        public void LoggerLogFileSizeShouldBeWithinRange()
        {
            var logger = GetTestLogger( logFolder: "MaxFileSizeTest", numberOfLogFiles: 10 );
            var expectedMaxFileSize = logger.LogConfiguration.MaxFileSize * 1024 * 1024;
            var onePercentVariation = .01;

            CreateLogFiles( logger, logger.LogConfiguration.MaxFileSize, logger.LogConfiguration.NumberOfLogFiles );

            logger.Close();

            var logFolderPath = System.IO.Path.GetFullPath( System.IO.Path.GetDirectoryName( logger.LogConfiguration.LogPath ) );

            Assert.That.FolderFileSizeIsWithinRange( logFolderPath, 0, expectedMaxFileSize, onePercentVariation );

            DeleteFilesInFolder( logFolderPath );
        }

        [TestMethod]
        public void LoggerVerboseShouldLogCorrectly()
        {
            var logger = GetTestLogger();

            var expectedLogMessages = new List<string>();
            var expectedSerializedConfig = TEST_LOGGER_CONFIG_SERIALIZATION.Replace( "{logPath}", logger.LogConfiguration.LogPath.Replace( "\\", "\\\\" ) );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( "CRM", logGuid );
            expectedLogMessages.Add( $"CRM {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( "CRM", $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"OTHER {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( "CRM", TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"CRM {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( "CRM", TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logger.Close();

            foreach ( var expectedMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, $" [VRB] {expectedMessage}" );
            }
        }

        [TestMethod]
        public void LoggerDebugShouldLogCorrectly()
        {
            var logger = GetTestLogger();

            var expectedLogMessages = new List<string>();
            var expectedSerializedConfig = TEST_LOGGER_CONFIG_SERIALIZATION.Replace( "{logPath}", logger.LogConfiguration.LogPath.Replace( "\\", "\\\\" ) );
            var logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( "CRM", logGuid );
            expectedLogMessages.Add( $"CRM {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( "CRM", $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"OTHER {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( "CRM", TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"CRM {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( "CRM", TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logger.Close();

            foreach ( var expectedMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, $" [DBG] {expectedMessage}" );
            }
        }

        [TestMethod]
        public void LoggerInformationShouldLogCorrectly()
        {
            var logger = GetTestLogger();

            var expectedLogMessages = new List<string>();
            var expectedSerializedConfig = TEST_LOGGER_CONFIG_SERIALIZATION.Replace( "{logPath}", logger.LogConfiguration.LogPath.Replace( "\\", "\\\\" ) );
            var logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( "CRM", logGuid );
            expectedLogMessages.Add( $"CRM {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( "CRM", $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"OTHER {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( "CRM", TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"CRM {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( "CRM", TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logger.Close();

            foreach ( var expectedMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, $" [INF] {expectedMessage}" );
            }
        }

        [TestMethod]
        public void LoggerWarningShouldLogCorrectly()
        {
            var logger = GetTestLogger();

            var expectedLogMessages = new List<string>();
            var expectedSerializedConfig = TEST_LOGGER_CONFIG_SERIALIZATION.Replace( "{logPath}", logger.LogConfiguration.LogPath.Replace( "\\", "\\\\" ) );
            var logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( "CRM", logGuid );
            expectedLogMessages.Add( $"CRM {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( "CRM", $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"OTHER {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( "CRM", TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"CRM {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( "CRM", TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logger.Close();

            foreach ( var expectedMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, $" [WRN] {expectedMessage}" );
            }
        }

        [TestMethod]
        public void LoggerErrorShouldLogCorrectly()
        {
            var logger = GetTestLogger();

            var expectedLogMessages = new List<string>();
            var expectedSerializedConfig = TEST_LOGGER_CONFIG_SERIALIZATION.Replace( "{logPath}", logger.LogConfiguration.LogPath.Replace( "\\", "\\\\" ) );
            var logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( "CRM", logGuid );
            expectedLogMessages.Add( $"CRM {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( "CRM", $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"OTHER {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( "CRM", TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"CRM {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( "CRM", TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logger.Close();

            foreach ( var expectedMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, $" [ERR] {expectedMessage}" );
            }
        }

        [TestMethod]
        public void LoggerFatalShouldLogCorrectly()
        {
            var logger = GetTestLogger();

            var expectedLogMessages = new List<string>();
            var expectedSerializedConfig = TEST_LOGGER_CONFIG_SERIALIZATION.Replace( "{logPath}", logger.LogConfiguration.LogPath.Replace( "\\", "\\\\" ) );
            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( "CRM", logGuid );
            expectedLogMessages.Add( $"CRM {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( "CRM", $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"OTHER {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( "CRM", TestException, $"{logGuid}" );
            expectedLogMessages.Add( $"CRM {logGuid}{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"OTHER {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( "CRM", TestException, $"{logGuid} {{@oneProperty}} {{@twoProperty}}", logger.LogConfiguration, RockLogLevel.All );
            expectedLogMessages.Add( $"CRM {logGuid} {expectedSerializedConfig} \"All\"{TEST_EXCEPTION_SERIALIZATION}" );

            logger.Close();

            foreach ( var expectedMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, $" [FTL] {expectedMessage}" );
            }
        }

        [TestMethod]
        public void LoggerVerboseShouldLogOnlySpecifiedDomains()
        {
            var logger = GetTestLogger( domainsToLog: new List<string> { "other" } );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            var expectedLogMessage = $"OTHER {logGuid}";

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( "CRM", logGuid );
            var excludedLogMessage = $"CRM {logGuid}";

            logger.Close();

            Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerDebugShouldLogOnlySpecifiedDomains()
        {
            var logger = GetTestLogger( domainsToLog: new List<string> { "other" } );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            var expectedLogMessage = $"OTHER {logGuid}";

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( "CRM", logGuid );
            var excludedLogMessage = $"CRM {logGuid}";

            logger.Close();

            Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerInformationShouldLogOnlySpecifiedDomains()
        {
            var logger = GetTestLogger( domainsToLog: new List<string> { "other" } );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            var expectedLogMessage = $"OTHER {logGuid}";

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( "CRM", logGuid );
            var excludedLogMessage = $"CRM {logGuid}";

            logger.Close();

            Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerWarningShouldLogOnlySpecifiedDomains()
        {
            var logger = GetTestLogger( domainsToLog: new List<string> { "other" } );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            var expectedLogMessage = $"OTHER {logGuid}";

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( "CRM", logGuid );
            var excludedLogMessage = $"CRM {logGuid}";

            logger.Close();

            Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerErrorShouldLogOnlySpecifiedDomains()
        {
            var logger = GetTestLogger( domainsToLog: new List<string> { "other" } );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            var expectedLogMessage = $"OTHER {logGuid}";

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( "CRM", logGuid );
            var excludedLogMessage = $"CRM {logGuid}";

            logger.Close();

            Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerFatalShouldLogOnlySpecifiedDomains()
        {
            var logger = GetTestLogger( domainsToLog: new List<string> { "other" } );

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            var expectedLogMessage = $"OTHER {logGuid}";

            logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( "CRM", logGuid );
            var excludedLogMessage = $"CRM {logGuid}";

            logger.Close();

            Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
        }

        [TestMethod]
        public void LoggerLogLevelOffShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.Off );

            var excludedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            Assert.That.FileNotFound( logger.LogConfiguration.LogPath );
        }

        [TestMethod]
        public void LoggerLogLevelFatalShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.Fatal );

            var expectedLogMessages = new List<string>();
            var excludedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            foreach ( var expectedLogMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            }

            foreach ( var excludedLogMessage in excludedLogMessages )
            {
                Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
            }
        }

        [TestMethod]
        public void LoggerLogLevelErrorShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.Error );

            var expectedLogMessages = new List<string>();
            var excludedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            foreach ( var expectedLogMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            }

            foreach ( var excludedLogMessage in excludedLogMessages )
            {
                Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
            }
        }

        [TestMethod]
        public void LoggerLogLevelWarningShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.Warning );

            var expectedLogMessages = new List<string>();
            var excludedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            foreach ( var expectedLogMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            }

            foreach ( var excludedLogMessage in excludedLogMessages )
            {
                Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
            }
        }

        [TestMethod]
        public void LoggerLogLevelInformationShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.Info );

            var expectedLogMessages = new List<string>();
            var excludedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            foreach ( var expectedLogMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            }

            foreach ( var excludedLogMessage in excludedLogMessages )
            {
                Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
            }
        }

        [TestMethod]
        public void LoggerLogLevelDebugShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.Debug );

            var expectedLogMessages = new List<string>();
            var excludedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            excludedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            foreach ( var expectedLogMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            }

            foreach ( var excludedLogMessage in excludedLogMessages )
            {
                Assert.That.FileDoesNotContains( logger.LogConfiguration.LogPath, excludedLogMessage );
            }
        }

        [TestMethod]
        public void LoggerLogLevelAllShouldLogCorrectly()
        {
            var logger = GetTestLogger( logLevel: RockLogLevel.All );

            var expectedLogMessages = new List<string>();

            var logGuid = $"{Guid.NewGuid()}";
            logger.Fatal( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Error( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Warning( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Information( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Debug( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logGuid = $"{Guid.NewGuid()}";
            logger.Verbose( logGuid );
            expectedLogMessages.Add( $"OTHER {logGuid}" );

            logger.Close();

            foreach ( var expectedLogMessage in expectedLogMessages )
            {
                Assert.That.FileContains( logger.LogConfiguration.LogPath, expectedLogMessage );
            }
        }

        #region Private Helper Code
        private static void DeleteFilesInFolder( string logFolder )
        {
            var logFolderPath = System.IO.Path.GetFullPath( logFolder );
            var files = System.IO.Directory.GetFiles( logFolderPath );
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
        }

        private Logger GetTestLogger( string logFolder = LOG_FOLDER, List<string> domainsToLog = null, RockLogLevel logLevel = RockLogLevel.All, int numberOfLogFiles = 2 )
        {
            if ( domainsToLog == null )
            {
                domainsToLog = new List<string> { "OTHER", "crm" };
            }

            var config = new RockLogConfiguration
            {
                LogLevel = logLevel,
                MaxFileSize = 1,
                NumberOfLogFiles = numberOfLogFiles,
                DomainsToLog = domainsToLog,
                LogPath = $"{logFolder}\\{Guid.NewGuid()}.log"
            };
            return new Logger( config );
        }

        private static void CreateLogFiles( Logger logger, int maxFilesizeInMB, int numberOfFiles )
        {
            var maxByteCount = maxFilesizeInMB * 1024 * 1024 * numberOfFiles;
            var currentByteCount = 0;
            var logHeaderInformation = "0000-00-00 00:00:00.000 -00:00 [INF] OTHER";

            while ( currentByteCount < maxByteCount )
            {
                var expectedLogMessage = $"Test - {Guid.NewGuid()}";
                logger.Information( expectedLogMessage );

                currentByteCount += Encoding.ASCII.GetByteCount( $"{logHeaderInformation} {expectedLogMessage}" );
            }
        }

        private class RockLogConfiguration : IRockLogConfiguration
        {
            public RockLogLevel LogLevel { get; set; }
            public int MaxFileSize { get; set; }
            public int NumberOfLogFiles { get; set; }
            public List<string> DomainsToLog { get; set; }
            public string LogPath { get; set; }
        }
        #endregion
    }
}
