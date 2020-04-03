using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Logging;
using Rock.Tests.Integration.Utility;

namespace Rock.Tests.Integration.Logging
{
    [TestClass]
    public class RockLogReaderTests
    {
        [TestMethod]
        public void RockLogReaderShouldReturnLogEntriesInCorrectOrder()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 3,
                DomainsToLog = new List<string> { "OTHER" },
                LogPath = $"\\Logs\\{Guid.NewGuid()}.log",
                LastUpdated = DateTime.Now
            };

            var logger = ReflectionHelper.InstantiateInternalObject<IRockLogger>( "Rock.Logging.RockLoggerSerilog", config );

            var expectedLogs = CreateLogFiles( logger );
            
            var rockReader = new RockLogReader( logger );

            var currentPageIndex = 0;
            var pageSize = 1000;
            var nextPageIndex = currentPageIndex + pageSize;

            var results = rockReader.GetEvents( currentPageIndex, pageSize );
            var lastIndex = expectedLogs.Count - 1;
            for ( var i = lastIndex; i >= 0; i-- )
            {
                if ( (lastIndex - i) >= nextPageIndex )
                {
                    currentPageIndex = nextPageIndex;
                    nextPageIndex = currentPageIndex + pageSize;
                    results = rockReader.GetEvents( currentPageIndex, pageSize );
                }

                var resultIndex = lastIndex - i - currentPageIndex;
                StringAssert.Contains( results[resultIndex].Message, expectedLogs[i] );
            }
            
        }

        [TestMethod]
        public void RockLogReaderShouldReturnNoResultsWithOutOfRangePageIndex()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 3,
                DomainsToLog = new List<string> { "OTHER" },
                LogPath = $"\\Logs\\{Guid.NewGuid()}.log",
                LastUpdated = DateTime.Now
            };

            var logger = ReflectionHelper.InstantiateInternalObject<IRockLogger>( "Rock.Logging.RockLoggerSerilog", config );

            var expectedLogs = CreateLogFiles( logger );

            var rockReader = new RockLogReader( logger );

            var currentPageIndex = 19000;
            var pageSize = 1000;
            var nextPageIndex = currentPageIndex + pageSize;

            var results = rockReader.GetEvents( currentPageIndex, pageSize );
            Assert.AreEqual( 0, results.Count );
        }

        [TestMethod]
        public void RockLogReaderShouldReturnAllResultsWithMaxPlusOnePageSize()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 3,
                DomainsToLog = new List<string> { "OTHER" },
                LogPath = $"\\Logs\\{Guid.NewGuid()}.log",
                LastUpdated = DateTime.Now
            };

            var logger = ReflectionHelper.InstantiateInternalObject<IRockLogger>( "Rock.Logging.RockLoggerSerilog", config );

            var expectedLogs = CreateLogFiles( logger );

            var rockReader = new RockLogReader( logger );

            var currentPageIndex = 0;
            var pageSize = 19000;
            var nextPageIndex = currentPageIndex + pageSize;

            var results = rockReader.GetEvents( currentPageIndex, pageSize );
            Assert.AreEqual( expectedLogs.Count, results.Count );
        }

        private List<string> CreateLogFiles( IRockLogger logger )
        {
            var maxByteCount = logger.LogConfiguration.MaxFileSize * 1024 * 1024 * (logger.LogConfiguration.NumberOfLogFiles - 1);
            var currentByteCount = 0;
            var logRecordSize = Encoding.ASCII.GetByteCount( "{\"@t\":\"0000-00-00T00:00:00.0000000Z\",\"@mt\":\"{domain} Test - 00000000-0000-0000-0000-000000000000\",\"domain\":\"OTHER\"}" );
            var expectedLogs = new List<string>();

            while ( currentByteCount < maxByteCount )
            {
                var guid = Guid.NewGuid();
                var expectedLogMessage = $"Test - {guid}";
                logger.Information( expectedLogMessage );
                expectedLogs.Add( guid.ToString() );
                currentByteCount += logRecordSize;
            }
            return expectedLogs;
        }
    }
}
