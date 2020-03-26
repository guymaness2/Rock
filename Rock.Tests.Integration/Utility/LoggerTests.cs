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
        public LoggerTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

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
                taskList.Add(Task.Run( () =>
                {
                    Logger.Log.Information( "coolPlugin", $"{logMessage} {i}." );
                } ));
            }
            var t = Task.WhenAll( taskList.ToArray() );
            await t;
        }

        private async Task ProcessLogs( int logCount, string logMessage )
        {
            var taskList = new List<Task>();

            for ( var i = 0; i < logCount; i++ )
            {
                taskList.Add( Task.Run( () =>
                {
                    Logger.Log.Information( $"{logMessage} {i}." );
                } ) );
            }
            var t = Task.WhenAll( taskList.ToArray() );
            await t;
        }
    }
}
