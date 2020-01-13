﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// SystemCommunication Service class
    /// </summary>
    public partial class SystemCommunicationService : Service<SystemCommunication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemCommunicationService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public SystemCommunicationService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( SystemCommunication item, out string errorMessage )
        {
            errorMessage = string.Empty;
 
            if ( new Service<GroupSync>( Context ).Queryable().Any( a => a.ExitSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupSync.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<GroupSync>( Context ).Queryable().Any( a => a.WelcomeSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupSync.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<GroupType>( Context ).Queryable().Any( a => a.ScheduleConfirmationSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<GroupType>( Context ).Queryable().Any( a => a.ScheduleReminderSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, GroupType.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<SignatureDocumentTemplate>( Context ).Queryable().Any( a => a.InviteSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, SignatureDocumentTemplate.FriendlyTypeName );
                return false;
            }  
 
            if ( new Service<WorkflowActionForm>( Context ).Queryable().Any( a => a.NotificationSystemCommunicationId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", SystemCommunication.FriendlyTypeName, WorkflowActionForm.FriendlyTypeName );
                return false;
            }  
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class SystemCommunicationExtensionMethods
    {
        /// <summary>
        /// Clones this SystemCommunication object to a new SystemCommunication object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static SystemCommunication Clone( this SystemCommunication source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as SystemCommunication;
            }
            else
            {
                var target = new SystemCommunication();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another SystemCommunication object to this SystemCommunication object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this SystemCommunication target, SystemCommunication source )
        {
            target.Id = source.Id;
            target.Bcc = source.Bcc;
            target.Body = source.Body;
            target.CategoryId = source.CategoryId;
            target.Cc = source.Cc;
            target.ForeignGuid = source.ForeignGuid;
            target.ForeignKey = source.ForeignKey;
            target.From = source.From;
            target.FromName = source.FromName;
            target.IsSystem = source.IsSystem;
            target.Subject = source.Subject;
            target.Title = source.Title;
            target.To = source.To;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
