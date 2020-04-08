// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
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
            var rockSettings = Rock.Web.SystemSettings.GetValue( SystemSetting.ROCK_LOGGING_SETTINGS ).FromJsonOrNull<RockLogSystemSettings>();

            if ( rockSettings == null )
            {
                LogLevel = RockLogLevel.Off;
                NumberOfLogFiles = 20;
                MaxFileSize = 20;
                DomainsToLog = new List<string>();
            }
            else
            {
                LogLevel = rockSettings.LogLevel;
                NumberOfLogFiles = rockSettings.NumberOfLogFiles;
                MaxFileSize = rockSettings.MaxFileSize;
                DomainsToLog = rockSettings.DomainsToLog;
            }

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
    }
}
