//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System.ComponentModel.Composition;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace Rock.REST.Groups
{
	/// <summary>
	/// REST WCF service for GroupTypes
	/// </summary>
    [Export(typeof(IService))]
    [ExportMetadata("RouteName", "Groups/GroupType")]
	[AspNetCompatibilityRequirements( RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed )]
    public partial class GroupTypeService : IGroupTypeService, IService
    {
		/// <summary>
		/// Gets a GroupType object
		/// </summary>
		[WebGet( UriTemplate = "{id}" )]
        public Rock.Groups.DTO.GroupType Get( string id )
        {
            var currentUser = Rock.CMS.UserService.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
				Rock.Groups.GroupType GroupType = GroupTypeService.Get( int.Parse( id ) );
				if ( GroupType.Authorized( "View", currentUser ) )
					return GroupType.DataTransferObject;
				else
					throw new WebFaultException<string>( "Not Authorized to View this GroupType", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Gets a GroupType object
		/// </summary>
		[WebGet( UriTemplate = "{id}/{apiKey}" )]
        public Rock.Groups.DTO.GroupType ApiGet( string id, string apiKey )
        {
            using (Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope())
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
					Rock.Groups.GroupType GroupType = GroupTypeService.Get( int.Parse( id ) );
					if ( GroupType.Authorized( "View", user.UserName ) )
						return GroupType.DataTransferObject;
					else
						throw new WebFaultException<string>( "Not Authorized to View this GroupType", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }
		
		/// <summary>
		/// Updates a GroupType object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}" )]
        public void UpdateGroupType( string id, Rock.Groups.DTO.GroupType GroupType )
        {
            var currentUser = Rock.CMS.UserService.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
				Rock.Groups.GroupType existingGroupType = GroupTypeService.Get( int.Parse( id ) );
				if ( existingGroupType.Authorized( "Edit", currentUser ) )
				{
					uow.objectContext.Entry(existingGroupType).CurrentValues.SetValues(GroupType);
					
					if (existingGroupType.IsValid)
						GroupTypeService.Save( existingGroupType, currentUser.PersonId );
					else
						throw new WebFaultException<string>( existingGroupType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this GroupType", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Updates a GroupType object
		/// </summary>
		[WebInvoke( Method = "PUT", UriTemplate = "{id}/{apiKey}" )]
        public void ApiUpdateGroupType( string id, string apiKey, Rock.Groups.DTO.GroupType GroupType )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
					Rock.Groups.GroupType existingGroupType = GroupTypeService.Get( int.Parse( id ) );
					if ( existingGroupType.Authorized( "Edit", user.UserName ) )
					{
						uow.objectContext.Entry(existingGroupType).CurrentValues.SetValues(GroupType);
					
						if (existingGroupType.IsValid)
							GroupTypeService.Save( existingGroupType, user.PersonId );
						else
							throw new WebFaultException<string>( existingGroupType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this GroupType", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Creates a new GroupType object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "" )]
        public void CreateGroupType( Rock.Groups.DTO.GroupType GroupType )
        {
            var currentUser = Rock.CMS.UserService.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
				Rock.Groups.GroupType existingGroupType = new Rock.Groups.GroupType();
				GroupTypeService.Add( existingGroupType, currentUser.PersonId );
				uow.objectContext.Entry(existingGroupType).CurrentValues.SetValues(GroupType);

				if (existingGroupType.IsValid)
					GroupTypeService.Save( existingGroupType, currentUser.PersonId );
				else
					throw new WebFaultException<string>( existingGroupType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
            }
        }

		/// <summary>
		/// Creates a new GroupType object
		/// </summary>
		[WebInvoke( Method = "POST", UriTemplate = "{apiKey}" )]
        public void ApiCreateGroupType( string apiKey, Rock.Groups.DTO.GroupType GroupType )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
					Rock.Groups.GroupType existingGroupType = new Rock.Groups.GroupType();
					GroupTypeService.Add( existingGroupType, user.PersonId );
					uow.objectContext.Entry(existingGroupType).CurrentValues.SetValues(GroupType);

					if (existingGroupType.IsValid)
						GroupTypeService.Save( existingGroupType, user.PersonId );
					else
						throw new WebFaultException<string>( existingGroupType.ValidationResults.AsDelimited(", "), System.Net.HttpStatusCode.BadRequest );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a GroupType object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}" )]
        public void DeleteGroupType( string id )
        {
            var currentUser = Rock.CMS.UserService.GetCurrentUser();
            if ( currentUser == null )
                throw new WebFaultException<string>("Must be logged in", System.Net.HttpStatusCode.Forbidden );

            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				uow.objectContext.Configuration.ProxyCreationEnabled = false;
				Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
				Rock.Groups.GroupType GroupType = GroupTypeService.Get( int.Parse( id ) );
				if ( GroupType.Authorized( "Edit", currentUser ) )
				{
					GroupTypeService.Delete( GroupType, currentUser.PersonId );
					GroupTypeService.Save( GroupType, currentUser.PersonId );
				}
				else
					throw new WebFaultException<string>( "Not Authorized to Edit this GroupType", System.Net.HttpStatusCode.Forbidden );
            }
        }

		/// <summary>
		/// Deletes a GroupType object
		/// </summary>
		[WebInvoke( Method = "DELETE", UriTemplate = "{id}/{apiKey}" )]
        public void ApiDeleteGroupType( string id, string apiKey )
        {
            using ( Rock.Data.UnitOfWorkScope uow = new Rock.Data.UnitOfWorkScope() )
            {
				Rock.CMS.UserService userService = new Rock.CMS.UserService();
                Rock.CMS.User user = userService.Queryable().Where( u => u.ApiKey == apiKey ).FirstOrDefault();

				if (user != null)
				{
					uow.objectContext.Configuration.ProxyCreationEnabled = false;
					Rock.Groups.GroupTypeService GroupTypeService = new Rock.Groups.GroupTypeService();
					Rock.Groups.GroupType GroupType = GroupTypeService.Get( int.Parse( id ) );
					if ( GroupType.Authorized( "Edit", user.UserName ) )
					{
						GroupTypeService.Delete( GroupType, user.PersonId );
						GroupTypeService.Save( GroupType, user.PersonId );
					}
					else
						throw new WebFaultException<string>( "Not Authorized to Edit this GroupType", System.Net.HttpStatusCode.Forbidden );
				}
				else
					throw new WebFaultException<string>( "Invalid API Key", System.Net.HttpStatusCode.Forbidden );
            }
        }

    }
}
