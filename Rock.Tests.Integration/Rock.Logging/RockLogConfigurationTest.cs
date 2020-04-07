using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Logging;
using Rock.SystemKey;
using Rock.Tests.Shared;

namespace Rock.Tests.Integration.Logging
{
    /// <summary>
    /// Summary description for RockLogConfiguration
    /// </summary>
    [TestClass]
    public class RockLogConfigurationTest
    {
        [TestMethod]
        public void RockLogConfigurationShouldLoadFromDatabase()
        {
            var expectedFileCount = 5;
            var expectedFileSize = 5;
            var expectedDomains = new List<string> { RockLogDomains.Other, RockLogDomains.Prayer, RockLogDomains.Group };
            var expectedLogLevel = RockLogLevel.Debug;

            RockLoggingHelpers.SaveRockLogConfiguration( expectedDomains, expectedLogLevel, expectedFileSize, expectedFileCount );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
            Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
            Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
            Assert.That.AreEqual( expectedLogLevel, rockLogConfig.LogLevel );
            Assert.That.AreEqual( expectedDomains, rockLogConfig.DomainsToLog );
        }

        [TestMethod]
        public void RockLogConfigurationInvalidShouldReturnDefaults()
        {
            var expectedFileCount = 20;
            var expectedFileSize = 20;
            var expectedDomains = new List<string>();
            var expectedLogLevel = RockLogLevel.Off;

            Rock.Web.SystemSettings.SetValue( SystemSetting.ROCK_LOGGING_SETTINGS, "garbage" );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
            Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
            Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
            Assert.That.AreEqual( expectedLogLevel, rockLogConfig.LogLevel );
            Assert.That.AreEqual( expectedDomains, rockLogConfig.DomainsToLog );
        }

        [TestMethod]
        public void RockLogConfigurationShouldLoadExpectedDomainsFromDatabase()
        {
            void AssertListIsCorrect( List<string> expectedDomains )
            {
                RockLoggingHelpers.SaveRockLogConfiguration( expectedDomains );

                var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

                Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
                Assert.That.AreEqual( expectedDomains, rockLogConfig.DomainsToLog );
            }

            AssertListIsCorrect( new List<string> { } );
            AssertListIsCorrect( new List<string> { RockLogDomains.Cms, RockLogDomains.Communications } );
            AssertListIsCorrect( new List<string> { RockLogDomains.Workflow,
                RockLogDomains.Streaks,
                RockLogDomains.Steps,
                RockLogDomains.Reporting,
                RockLogDomains.Prayer,
                RockLogDomains.Other,
                RockLogDomains.Jobs,
                RockLogDomains.Group,
                RockLogDomains.Finance,
                RockLogDomains.Event,
                RockLogDomains.Crm,
                RockLogDomains.Core,
                RockLogDomains.Connection,
                RockLogDomains.Communications,
                RockLogDomains.Cms } );
            AssertListIsCorrect( new List<string> { "Custom Domain 1", "Custom Domain 2" } );
        }

        [TestMethod]
        public void RockLogConfigurationShouldCorrectlyLoadFromDatabase()
        {
            var expectedLogLevel = RockLogLevel.Warning;
            var expectedFileSize = 5;
            var expectedFileCount = 10;
            var expectedDomains = new List<string> { };

            RockLoggingHelpers.SaveRockLogConfiguration( expectedDomains, expectedLogLevel, expectedFileSize, expectedFileCount );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
            Assert.That.AreEqual( expectedLogLevel, rockLogConfig.LogLevel );
            Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
            Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
            Assert.That.AreEqual( expectedDomains, rockLogConfig.DomainsToLog );
        }

        [TestMethod]
        public void RockLogConfigurationShouldAutomaticallyUpdateIfDatabaseChanged()
        {
            var originalLogLevel = RockLogLevel.Warning;
            var originalFileSize = 5;
            var originalFileCount = 10;
            var originalDomains = new List<string> { RockLogDomains.Cms, RockLogDomains.Communications };

            var expectedLogLevel = RockLogLevel.All;
            var expectedFileSize = 25;
            var expectedFileCount = 30;
            var expectedDomains = new List<string> { };

            RockLoggingHelpers.SaveRockLogConfiguration( originalDomains, originalLogLevel, originalFileSize, originalFileCount );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );

            RockLoggingHelpers.SaveRockLogConfiguration( expectedDomains, expectedLogLevel, expectedFileSize, expectedFileCount );

            Assert.That.AreEqual( expectedLogLevel, rockLogConfig.LogLevel );
            Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
            Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
            Assert.That.AreEqual( expectedDomains, rockLogConfig.DomainsToLog );
        }
    }
}
