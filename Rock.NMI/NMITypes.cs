using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rock.NMI
{

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum NMIPaymentType
    {
        /// <summary>
        /// The card
        /// </summary>
        card,

        /// <summary>
        /// The ach
        /// </summary>
        ach
    }

    public class BaseResponse
    {
        /// <summary>
        /// Newtonsoft.Json.JsonExtensionData instructs the Newtonsoft.Json.JsonSerializer to deserialize properties with no
        /// matching class member into the specified collection
        /// </summary>
        /// <value>
        /// The other data.
        /// </value>
        [Newtonsoft.Json.JsonExtensionData( ReadData = true, WriteData = false )]
        public IDictionary<string, Newtonsoft.Json.Linq.JToken> _additionalData { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class TokenizerResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [JsonProperty( "message" )]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the card.
        /// </summary>
        /// <value>
        /// The card.
        /// </value>
        [JsonProperty( "card" )]
        public CardTokenResponse Card { get; set; }

        /// <summary>
        /// Gets or sets the check.
        /// </summary>
        /// <value>
        /// The check.
        /// </value>
        [JsonProperty( "check" )]
        public CheckTokenResponse Check { get; set; }

        /// <summary>
        /// Determines whether [is success status].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is success status]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSuccessStatus() => ErrorMessage.IsNullOrWhiteSpace() && ValidationMessage.IsNullOrWhiteSpace();

        /// <summary>
        /// Determines whether [has validation error].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has validation error]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValidationError() => ValidationMessage.IsNotNullOrWhiteSpace();

        /* We added these things since NMI doesn't really do it and we had to do custom stuff in gatewayCollect.js to populate these */
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        /// <value>
        /// The validation message.
        /// </value>
        [JsonProperty( "validationMessage" )]
        public string ValidationMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        [JsonProperty( "errorMessage" )]
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CardTokenResponse
    {
        [JsonProperty( "number" )]
        public string Number { get; set; }

        [JsonProperty( "bin" )]
        public string Bin { get; set; }

        [JsonProperty( "exp" )]
        public string Exp { get; set; }

        [JsonProperty( "hash" )]
        public string Hash { get; set; }

        [JsonProperty( "type" )]
        public string Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckTokenResponse
    {
        [JsonProperty( "name" )]
        public string Name { get; set; }

        [JsonProperty( "account" )]
        public string Account { get; set; }

        [JsonProperty( "hash" )]
        public string Hash { get; set; }

        [JsonProperty( "aba" )]
        public string Aba { get; set; }
    }

    /* Customer Vault Response Classes */

    public class CustomerVaultQueryResponse
    {
        [JsonProperty( "customer_vault" )]
        public CustomerVault CustomerVault { get; set; }
    }

    public class CustomerVault
    {
        [JsonProperty( "customer" )]
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        [JsonProperty( "id" )]
        public string Id { get; set; }

        [JsonProperty( "first_name" )]
        public string FirstName { get; set; }

        [JsonProperty( "last_name" )]
        public string LastName { get; set; }

        [JsonProperty( "address_1" )]
        public string Address1 { get; set; }

        [JsonProperty( "address_2" )]
        public string Address2 { get; set; }

        [JsonProperty( "company" )]
        public string Company { get; set; }

        [JsonProperty( "city" )]
        public string City { get; set; }

        [JsonProperty( "state" )]
        public string State { get; set; }

        [JsonProperty( "postal_code" )]
        public string PostalCode { get; set; }

        [JsonProperty( "country" )]
        public string Country { get; set; }

        [JsonProperty( "email" )]
        public string Email { get; set; }

        [JsonProperty( "phone" )]
        public string Phone { get; set; }

        [JsonProperty( "cc_number" )]
        public string CcNumber { get; set; }

        [JsonProperty( "cc_hash" )]
        public string CcHash { get; set; }

        [JsonProperty( "cc_exp" )]
        public string CcExp { get; set; }

        [JsonProperty( "cc_start_date" )]
        public string CcStartDate { get; set; }

        [JsonProperty( "cc_issue_number" )]
        public string CCIssueNumber { get; set; }

        [JsonProperty( "check_account" )]
        public string CheckAccount { get; set; }

        [JsonProperty( "check_hash" )]
        public string CheckHash { get; set; }

        [JsonProperty( "check_aba" )]
        public string CheckAba { get; set; }

        [JsonProperty( "check_name" )]
        public string CheckName { get; set; }

        [JsonProperty( "account_holder_type" )]
        public string AccountHolderType { get; set; }

        [JsonProperty( "account_type" )]
        public string AccountType { get; set; }

        [JsonProperty( "sec_code" )]
        public string SecCode { get; set; }

        [JsonProperty( "processor_id" )]
        public string ProcessorId { get; set; }

        [JsonProperty( "cc_bin" )]
        public string CcBin { get; set; }

        [JsonProperty( "cc_type" )]
        public string CcType { get; set; }

        [JsonProperty( "created" )]
        public string Created { get; set; }

        [JsonProperty( "updated" )]
        public string Updated { get; set; }

        [JsonProperty( "account_updated" )]
        public string AccountUpdated { get; set; }

        [JsonProperty( "customer_vault_id" )]
        public string CustomerVaultId { get; set; }

        /// <summary>
        /// Newtonsoft.Json.JsonExtensionData instructs the Newtonsoft.Json.JsonSerializer to deserialize properties with no
        /// matching class member into the specified collection
        /// </summary>
        /// <value>
        /// The other data.
        /// </value>
        [Newtonsoft.Json.JsonExtensionData( ReadData = true, WriteData = false )]
        public IDictionary<string, Newtonsoft.Json.Linq.JToken> _additionalData { get; set; }
    }

    /* Charge Response */

    public class ChargeResponse
    {
        [JsonProperty( "response" )]
        public string Response { get; set; }

        [JsonProperty( "responsetext" )]
        public string ResponseText { get; set; }

        [JsonProperty( "authcode" )]
        public string AuthCode { get; set; }

        [JsonProperty( "transactionid" )]
        public string TransactionId { get; set; }

        [JsonProperty( "avsresponse" )]
        public string AVSResponse { get; set; }

        [JsonProperty( "cvvresponse" )]
        public string CVVResponse { get; set; }

        [JsonProperty( "orderid" )]
        public string OrderId { get; set; }

        [JsonProperty( "type" )]
        public string Type { get; set; }

        [JsonProperty( "response_code" )]
        public string ResponseCode { get; set; }

        [JsonProperty( "customer_vault_id" )]
        public string CustomerVaultId { get; set; }

        /// <summary>
        /// Newtonsoft.Json.JsonExtensionData instructs the Newtonsoft.Json.JsonSerializer to deserialize properties with no
        /// matching class member into the specified collection
        /// </summary>
        /// <value>
        /// The other data.
        /// </value>
        [Newtonsoft.Json.JsonExtensionData( ReadData = true, WriteData = false )]
        public IDictionary<string, Newtonsoft.Json.Linq.JToken> _additionalData { get; set; }
    }

    /* Create Customer  */
    public class CreateCustomerResponse
    {
        [JsonProperty( "response" )]
        public string Response { get; set; }

        [JsonProperty( "responsetext" )]
        public string ResponseText { get; set; }

        [JsonProperty( "authcode" )]
        public string AuthCode { get; set; }

        [JsonProperty( "transactionid" )]
        public string TransactionId { get; set; }

        [JsonProperty( "avsresponse" )]
        public string AVSResponse { get; set; }

        [JsonProperty( "cvvresponse" )]
        public string CVVResponse { get; set; }

        [JsonProperty( "orderid" )]
        public string OrderId { get; set; }

        [JsonProperty( "type" )]
        public string Type { get; set; }

        [JsonProperty( "response_code" )]
        public string ResponseCode { get; set; }

        [JsonProperty( "customer_vault_id" )]
        public string CustomerVaultId { get; set; }
    }


    /* Get Payments Response */
}
