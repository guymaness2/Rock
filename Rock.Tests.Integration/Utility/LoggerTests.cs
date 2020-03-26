using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Utility;
using System.Threading.Tasks;

namespace Rock.Tests.Integration.Utility
{
    /// <summary>
    /// Summary description for LoggerTests
    /// </summary>
    [TestClass]
    public class LoggerTests
    {
        class RockLogConfiguration : IRockLogConfiguration
        {
            public RockLogLevel LogLevel { get; set; }
            public int MaxFileSize { get; set; }
            public int NumberOfLogFiles { get; set; }
            public List<string> DomainsToLog { get; set; }
        }

        [TestMethod]
        public void LogInformationShouldOnlyLogCorrectDomains()
        {
            var config = new RockLogConfiguration
            {
                LogLevel = RockLogLevel.All,
                MaxFileSize = 1,
                NumberOfLogFiles = 1,
                DomainsToLog = new List<string> { "OTHER" }
            };
            var logger = new Logger( config );
            logger.Information( "Test" );
            logger.Information( "CRM", "CRM Test" );
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
                taskList.Add(Task.Run( () => Logger.Log.Information( "coolPlugin", $"{logMessage} {index}." )));
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
