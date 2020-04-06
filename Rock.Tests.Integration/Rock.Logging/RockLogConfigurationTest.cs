using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Logging;
using Rock.SystemKey;
using Rock.Tests.Integration.Utility;
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
        public void RockLogConfigurationShouldLogValidLogLevelFromDatabase()
        {
            var availableLogLevels = Enum.GetValues( typeof( RockLogLevel ) );

            foreach ( var expectedLogLevel in availableLogLevels )
            {
                Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_LOG_LEVEL, expectedLogLevel.ToString() );

                var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

                Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
                Assert.That.AreEqual( expectedLogLevel, rockLogConfig.LogLevel );
            }
        }

        [TestMethod]
        public void RockLogConfigurationInvalidLogLevelShouldReturnOff()
        {
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_LOG_LEVEL, "garbage" );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
            Assert.That.AreEqual( RockLogLevel.Off, rockLogConfig.LogLevel );
        }

        [TestMethod]
        public void RockLogConfigurationShouldLogValidFileSizeFromDatabase()
        {
            for ( var i = 0; i < 100; i += 5 )
            {
                var expectedFileSize = i;

                Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_SIZE, expectedFileSize.ToString() );

                var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

                Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
                Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
            }
        }

        [TestMethod]
        public void RockLogConfigurationInvalidFilesizeShouldReturn0()
        {
            var expectedFileSize = 0;

            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_SIZE, "garbage" );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
            Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
        }

        [TestMethod]
        public void RockLogConfigurationShouldLogValidFileCountFromDatabase()
        {
            for ( var i = 0; i < 100; i += 5 )
            {
                var expectedFileCount = i;

                Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_COUNT, expectedFileCount.ToString() );

                var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

                Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
                Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
            }
        }

        [TestMethod]
        public void RockLogConfigurationInvalidFileCountShouldReturn0()
        {
            var expectedFileCount = 0;

            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_COUNT, "garbage" );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );
            Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
        }

        [TestMethod]
        public void RockLogConfigurationShouldLoadExpectedDomainsFromDatabase()
        {
            void AssertListIsCorrect( List<string> expectedDomains )
            {
                Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_DOMAINS_TO_LOG, string.Join( ",", expectedDomains ) );

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

            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_LOG_LEVEL, expectedLogLevel.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_SIZE, expectedFileSize.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_COUNT, expectedFileCount.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_DOMAINS_TO_LOG, string.Join( ",", expectedDomains ) );

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

            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_LOG_LEVEL, originalLogLevel.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_SIZE, originalFileSize.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_COUNT, originalFileCount.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_DOMAINS_TO_LOG, string.Join( ",", originalDomains ) );

            var rockLogConfig = ReflectionHelper.InstantiateInternalObject<IRockLogConfiguration>( "Rock.Logging.RockLogConfiguration" );

            Assert.That.IsNotNull( rockLogConfig, "Rock Log Configuration was not created." );

            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_LOG_LEVEL, expectedLogLevel.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_SIZE, expectedFileSize.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_FILE_COUNT, expectedFileCount.ToString() );
            Rock.Web.SystemSettings.SetValue( SystemSetting.LOGGING_DOMAINS_TO_LOG, string.Join( ",", expectedDomains ) );

            Assert.That.AreEqual( expectedLogLevel, rockLogConfig.LogLevel );
            Assert.That.AreEqual( expectedFileSize, rockLogConfig.MaxFileSize );
            Assert.That.AreEqual( expectedFileCount, rockLogConfig.NumberOfLogFiles );
            Assert.That.AreEqual( expectedDomains, rockLogConfig.DomainsToLog );
        }
    }
}
