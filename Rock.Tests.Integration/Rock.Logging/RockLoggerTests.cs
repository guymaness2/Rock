using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Logging;
using Rock.Tests.Shared;

namespace Rock.Tests.Integration.Logging
{
    [TestClass]
    public class RockLoggerTests
    {
        [TestMethod]
        public void ConfirmRockLoggerLogsCorrectly()
        {
            var originalLogLevel = RockLogLevel.All;
            var originalFileSize = 5;
            var originalFileCount = 10;
            var originalDomains = new List<string> { RockLogDomains.Other, RockLogDomains.Communications };

            RockLoggingHelpers.SaveRockLogConfiguration( originalDomains, originalLogLevel, originalFileSize, originalFileCount );

            var expectedMessage = $"Test {Guid.NewGuid()}";
            RockLogger.Log.Information( expectedMessage );
            RockLogger.Log.Close();
            Assert.That.FileContains( RockLogger.Log.LogConfiguration.LogPath, expectedMessage );
        }

        [TestCleanup]
        public void Cleanup()
        {
            var folder = System.IO.Path.GetDirectoryName( RockLogger.Log.LogConfiguration.LogPath );
            DeleteFilesInFolder( folder );
        }

        private void DeleteFilesInFolder( string logFolder )
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
