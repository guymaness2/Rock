﻿// <copyright>
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
namespace Rock.Migrations
{
    using Rock.Logging;
    using Rock.SystemKey;
    using Rock.Utility;

    /// <summary>
    ///
    /// </summary>
    public partial class AddRockLoggingConfigurationAndViewer : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddRockLogLevelDefinedValues();

            AddRockLogSystemSettings();

            AddLogPage();
        }

        private void AddRockLogLevelDefinedValues()
        {
            RockMigrationHelper.AddDefinedType( "System Settings",
                "Logging Domains",
                "Domains that can be logged.",
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "CMS",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CMS );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Event",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_EVENT );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Reporting",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_REPORTING );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Communications",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_COMMUNICATIONS );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Finance",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_FINANCE );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Steps",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_STEPS );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Connection",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CONNECTION );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Group",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_GROUP );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Streaks",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_STREAKS );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Core",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CORE );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Jobs",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_JOBS );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Workflow",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_WORKFLOW );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "CRM",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CRM );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Prayer",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_PRAYER );

            RockMigrationHelper.UpdateDefinedValue(
                Rock.SystemGuid.DefinedType.LOGGING_DOMAINS,
                "Other",
                string.Empty,
                Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_OTHER );
        }
        private void AddRockLogSystemSettings()
        {
            var defaultRockLogSystemSettings = new RockLogSystemSettings
            {
                DomainsToLog = new System.Collections.Generic.List<string>(),
                LogLevel = RockLogLevel.Off,
                NumberOfLogFiles = 20,
                MaxFileSize = 20
            };

            var serializedRockLog = defaultRockLogSystemSettings.ToJson();

            RockMigrationHelper.AddGlobalAttribute( Rock.SystemGuid.FieldType.TEXT,
                Rock.Model.Attribute.SYSTEM_SETTING_QUALIFIER,
                string.Empty,
                SystemSetting.ROCK_LOGGING_SETTINGS,
                "Rock Logging System Settings",
                0,
                serializedRockLog,
                "B9D4A315-8672-4214-B5D3-01A06C3CAD9F",
                SystemSetting.ROCK_LOGGING_SETTINGS );
        }

        private void AddLogPage()
        {
            RockMigrationHelper.UpdateBlockType( "Logs", "Block to view system logs.", "~/Blocks/Administration/LogViewer.ascx", "Administration", "6059FC03-E398-4359-8632-909B63FFA550" );

            RockMigrationHelper.AddPage( true, "C831428A-6ACD-4D49-9B2D-046D399E3123", "D65F783D-87A9-4CC9-8110-E83466A0EADB", "Rock Logs", "", "82EC7718-6549-4531-A0AB-7957919AE71C", "fa fa-file-medical-alt" ); // Site:Rock RMS

            // Add Block to Page: Rock Logs Site: Rock RMS
            RockMigrationHelper.AddBlock( true, "82EC7718-6549-4531-A0AB-7957919AE71C".AsGuid(), null, "C2D29296-6A87-47A9-A753-EE4E9159C4C4".AsGuid(), "6059FC03-E398-4359-8632-909B63FFA550".AsGuid(), "Rock Logs", "Main", @"", @"", 0, "BDEF9AA0-55FC-4A66-8938-2AB2E075521B" );
        }

        private void RemoveRockLogLevelDefinedValues()
        {
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CMS );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_EVENT );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_REPORTING );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_COMMUNICATIONS );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_FINANCE );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_STEPS );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CONNECTION );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_GROUP );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_STREAKS );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CORE );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_JOBS );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_WORKFLOW );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_CRM );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_PRAYER );
            RockMigrationHelper.DeleteDefinedValue( Rock.SystemGuid.DefinedValue.LOGGING_DOMAIN_OTHER );

            RockMigrationHelper.DeleteDefinedType( Rock.SystemGuid.DefinedType.LOGGING_DOMAINS );
        }

        private void RemoveRockLogSystemSettings()
        {
            RockMigrationHelper.DeleteAttribute( "B9D4A315-8672-4214-B5D3-01A06C3CAD9F" );
        }

        private void RemoveLogPage()
        {
            // Remove Block: Rock Logs, from Page: Rock Logs, Site: Rock RMS
            RockMigrationHelper.DeleteBlock( "BDEF9AA0-55FC-4A66-8938-2AB2E075521B" );

            RockMigrationHelper.DeletePage( "82EC7718-6549-4531-A0AB-7957919AE71C" ); //  Page: Rock Logs, Layout: Full Width, Site: Rock RMS

            RockMigrationHelper.DeleteBlockType( "6059FC03-E398-4359-8632-909B63FFA550" ); // Logs
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            RemoveRockLogSystemSettings();

            RemoveRockLogLevelDefinedValues();

            RemoveLogPage();
        }
    }
}
