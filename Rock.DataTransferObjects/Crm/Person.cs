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
using System;

namespace Rock.CRM.DTO
{
    /// <summary>
    /// Person Data Transfer Object.
    /// </summary>
	/// <remarks>
	/// Data Transfer Objects are a lightweight version of the Entity object that are used
	/// in situations like serializing the object in the REST api
	/// </remarks>
    public partial class Person
    {
        /// <summary>
        /// The Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; set; }

		/// <summary>
		/// Gets or sets the System.
		/// </summary>
		/// <value>
		/// System.
		/// </value>
		public bool System { get; set; }

		/// <summary>
		/// Gets or sets the Given Name.
		/// </summary>
		/// <value>
		/// Given Name.
		/// </value>
		public string GivenName { get; set; }

		/// <summary>
		/// Gets or sets the Nick Name.
		/// </summary>
		/// <value>
		/// Nick Name.
		/// </value>
		public string NickName { get; set; }

		/// <summary>
		/// Gets or sets the Last Name.
		/// </summary>
		/// <value>
		/// Last Name.
		/// </value>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the Gender.
		/// </summary>
		/// <value>
		/// Enum[Gender].
		/// </value>
		public int? Gender { get; set; }

		/// <summary>
		/// Gets or sets the Email.
		/// </summary>
		/// <value>
		/// Email.
		/// </value>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the Birth Month.
		/// </summary>
		/// <value>
		/// Birth Month.
		/// </value>
		public int? BirthMonth { get; set; }

		/// <summary>
		/// Gets or sets the Birth Day.
		/// </summary>
		/// <value>
		/// Birth Day.
		/// </value>
		public int? BirthDay { get; set; }

		/// <summary>
		/// Gets or sets the Birth Year.
		/// </summary>
		/// <value>
		/// Birth Year.
		/// </value>
		public int? BirthYear { get; set; }

		/// <summary>
		/// Gets or sets the Created Date Time.
		/// </summary>
		/// <value>
		/// Created Date Time.
		/// </value>
		public DateTime? CreatedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the Modified Date Time.
		/// </summary>
		/// <value>
		/// Modified Date Time.
		/// </value>
		public DateTime? ModifiedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the Created By Person Id.
		/// </summary>
		/// <value>
		/// Created By Person Id.
		/// </value>
		public int? CreatedByPersonId { get; set; }

		/// <summary>
		/// Gets or sets the Modified By Person Id.
		/// </summary>
		/// <value>
		/// Modified By Person Id.
		/// </value>
		public int? ModifiedByPersonId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDTO"/> class.
        /// </summary>
		public Person()
		{
		}
	}
}
