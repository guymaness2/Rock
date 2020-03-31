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
    using Rock.SystemKey;
    using Rock.Utility;

    /// <summary>
    ///
    /// </summary>
    public partial class AddLoggingDefinedTypesAndAttributes : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
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

            RockMigrationHelper.AddGlobalAttribute( Rock.SystemGuid.FieldType.TEXT,
                Rock.Model.Attribute.SYSTEM_SETTING_QUALIFIER,
                string.Empty,
                SystemSetting.LOGGING_LOG_LEVEL,
                "Logging log level.",
                0,
                RockLogLevel.Off.ToString(),
                "E183CD5B-5820-4C4A-9462-E5F285C325B2",
                SystemSetting.LOGGING_LOG_LEVEL );

            RockMigrationHelper.AddGlobalAttribute( Rock.SystemGuid.FieldType.TEXT,
                Rock.Model.Attribute.SYSTEM_SETTING_QUALIFIER,
                string.Empty,
                SystemSetting.LOGGING_DOMAINS_TO_LOG,
                "Logging domain to log.",
                0,
                string.Empty,
                "9E896855-992D-4E63-B358-8190DAA356F8",
                SystemSetting.LOGGING_DOMAINS_TO_LOG );

            RockMigrationHelper.AddGlobalAttribute( Rock.SystemGuid.FieldType.TEXT,
                Rock.Model.Attribute.SYSTEM_SETTING_QUALIFIER,
                string.Empty,
                SystemSetting.LOGGING_FILE_COUNT,
                "Logging number of log files to keep.",
                0,
                "20",
                "381460BE-5435-44B6-AFB7-E5F40FFECB59",
                SystemSetting.LOGGING_FILE_COUNT );

            RockMigrationHelper.AddGlobalAttribute( Rock.SystemGuid.FieldType.TEXT,
                Rock.Model.Attribute.SYSTEM_SETTING_QUALIFIER,
                string.Empty,
                SystemSetting.LOGGING_FILE_SIZE,
                "Logging the max size of a log file.",
                0,
                "20",
                "5BB23964-94C7-4D87-AE5F-218D6950605C",
                SystemSetting.LOGGING_FILE_SIZE );
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            RockMigrationHelper.DeleteAttribute( "E183CD5B-5820-4C4A-9462-E5F285C325B2" );
            RockMigrationHelper.DeleteAttribute( "9E896855-992D-4E63-B358-8190DAA356F8" );
            RockMigrationHelper.DeleteAttribute( "381460BE-5435-44B6-AFB7-E5F40FFECB59" );
            RockMigrationHelper.DeleteAttribute( "5BB23964-94C7-4D87-AE5F-218D6950605C" );

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
    }
}
