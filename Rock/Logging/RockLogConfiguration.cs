using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rock.SystemKey;

namespace Rock.Logging
{
    internal class RockLogConfiguration : IRockLogConfiguration
    {
        private RockLogLevel logLevel;
        public RockLogLevel LogLevel
        {
            get
            {
                UpdateConfigIfRequired();
                return logLevel;
            }
            set
            {
                logLevel = value;
            }
        }

        private int maxFileSize;
        public int MaxFileSize
        {
            get
            {
                UpdateConfigIfRequired();
                return maxFileSize;
            }
            set
            {
                maxFileSize = value;
            }
        }

        private int numberOfLogFiles;
        public int NumberOfLogFiles
        {
            get
            {
                UpdateConfigIfRequired();
                return numberOfLogFiles;
            }
            set
            {
                numberOfLogFiles = value;
            }
        }

        private List<string> domainsToLog;
        public List<string> DomainsToLog
        {
            get
            {
                UpdateConfigIfRequired();
                return domainsToLog;
            }
            set
            {
                domainsToLog = value;
            }
        }

        public DateTime LastUpdated { get; set; }

        public string LogPath { get; set; }

        public RockLogConfiguration()
        {
            UpdateConfigFromSystemSettings();
        }

        private void UpdateConfigFromSystemSettings()
        {
            LogLevel = GetRockLogLevel();

            NumberOfLogFiles = Rock.Web.SystemSettings.GetValue( SystemSetting.LOGGING_FILE_COUNT ).AsInteger();
            MaxFileSize = Rock.Web.SystemSettings.GetValue( SystemSetting.LOGGING_FILE_SIZE ).AsInteger();

            DomainsToLog = Rock.Web.SystemSettings
                            .GetValue( SystemSetting.LOGGING_DOMAINS_TO_LOG )
                            .Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries )
                            .ToList();

            LogPath = System.IO.Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "App_Data\\Logs\\Rock.log" );

            LastUpdated = DateTime.Now;
        }

        private void UpdateConfigIfRequired()
        {
            if ( LastUpdated < Rock.Web.SystemSettings.LastUpdated )
            {
                UpdateConfigFromSystemSettings();
            }
        }

        private RockLogLevel GetRockLogLevel()
        {
            var currentLogLevel = Rock.Web.SystemSettings.GetValue( SystemSetting.LOGGING_LOG_LEVEL );

            try
            {
                var logLevel = Enum.Parse( typeof( RockLogLevel ), currentLogLevel );
                return ( RockLogLevel ) logLevel;
            }
            catch
            {
                return RockLogLevel.Off;
            }

        }
    }
}
