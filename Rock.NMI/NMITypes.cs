using System;
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

    /// <summary>
    /// Base Response for most classes. All this really does is give <see cref="_additionalData"/>
    /// </summary>
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

        /// <summary>
        /// Gets or sets the validation message.
        /// We added these things since NMI doesn't really do it and we had to do custom stuff in gatewayCollect.js to populate these
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
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        [JsonProperty( "number" )]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the bin.
        /// </summary>
        /// <value>
        /// The bin.
        /// </value>
        [JsonProperty( "bin" )]
        public string Bin { get; set; }

        /// <summary>
        /// Gets or sets the exp.
        /// </summary>
        /// <value>
        /// The exp.
        /// </value>
        [JsonProperty( "exp" )]
        public string Exp { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>
        /// The hash.
        /// </value>
        [JsonProperty( "hash" )]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty( "type" )]
        public string Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckTokenResponse
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty( "name" )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        [JsonProperty( "account" )]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>
        /// The hash.
        /// </value>
        [JsonProperty( "hash" )]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the aba.
        /// </summary>
        /// <value>
        /// The aba.
        /// </value>
        [JsonProperty( "aba" )]
        public string Aba { get; set; }
    }

    /* Customer Vault Response Classes */

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class CustomerVaultQueryResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the customer vault.
        /// </summary>
        /// <value>
        /// The customer vault.
        /// </value>
        [JsonProperty( "customer_vault" )]
        public CustomerVault CustomerVault { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class CustomerVault : BaseResponse
    {
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        [JsonProperty( "customer" )]
        public Customer Customer { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class Customer : BaseResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty( "id" )]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [JsonProperty( "first_name" )]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [JsonProperty( "last_name" )]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the address1.
        /// </summary>
        /// <value>
        /// The address1.
        /// </value>
        [JsonProperty( "address_1" )]
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the address2.
        /// </summary>
        /// <value>
        /// The address2.
        /// </value>
        [JsonProperty( "address_2" )]
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        [JsonProperty( "company" )]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [JsonProperty( "city" )]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [JsonProperty( "state" )]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        [JsonProperty( "postal_code" )]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        [JsonProperty( "country" )]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [JsonProperty( "email" )]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        [JsonProperty( "phone" )]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the cc number.
        /// </summary>
        /// <value>
        /// The cc number.
        /// </value>
        [JsonProperty( "cc_number" )]
        public string CcNumber { get; set; }

        /// <summary>
        /// Gets or sets the cc hash.
        /// </summary>
        /// <value>
        /// The cc hash.
        /// </value>
        [JsonProperty( "cc_hash" )]
        public string CcHash { get; set; }

        /// <summary>
        /// Gets or sets the cc exp.
        /// </summary>
        /// <value>
        /// The cc exp.
        /// </value>
        [JsonProperty( "cc_exp" )]
        public string CcExp { get; set; }

        /// <summary>
        /// Gets or sets the cc start date.
        /// </summary>
        /// <value>
        /// The cc start date.
        /// </value>
        [JsonProperty( "cc_start_date" )]
        public string CcStartDate { get; set; }

        /// <summary>
        /// Gets or sets the cc issue number.
        /// </summary>
        /// <value>
        /// The cc issue number.
        /// </value>
        [JsonProperty( "cc_issue_number" )]
        public string CCIssueNumber { get; set; }

        /// <summary>
        /// Gets or sets the check account.
        /// </summary>
        /// <value>
        /// The check account.
        /// </value>
        [JsonProperty( "check_account" )]
        public string CheckAccount { get; set; }

        /// <summary>
        /// Gets or sets the check hash.
        /// </summary>
        /// <value>
        /// The check hash.
        /// </value>
        [JsonProperty( "check_hash" )]
        public string CheckHash { get; set; }

        /// <summary>
        /// Gets or sets the check aba.
        /// </summary>
        /// <value>
        /// The check aba.
        /// </value>
        [JsonProperty( "check_aba" )]
        public string CheckAba { get; set; }

        /// <summary>
        /// Gets or sets the name of the check.
        /// </summary>
        /// <value>
        /// The name of the check.
        /// </value>
        [JsonProperty( "check_name" )]
        public string CheckName { get; set; }

        /// <summary>
        /// Gets or sets the type of the account holder.
        /// </summary>
        /// <value>
        /// The type of the account holder.
        /// </value>
        [JsonProperty( "account_holder_type" )]
        public string AccountHolderType { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <value>
        /// The type of the account.
        /// </value>
        [JsonProperty( "account_type" )]
        public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the sec code.
        /// </summary>
        /// <value>
        /// The sec code.
        /// </value>
        [JsonProperty( "sec_code" )]
        public string SecCode { get; set; }

        /// <summary>
        /// Gets or sets the processor identifier.
        /// </summary>
        /// <value>
        /// The processor identifier.
        /// </value>
        [JsonProperty( "processor_id" )]
        public string ProcessorId { get; set; }

        /// <summary>
        /// Gets or sets the cc bin.
        /// </summary>
        /// <value>
        /// The cc bin.
        /// </value>
        [JsonProperty( "cc_bin" )]
        public string CcBin { get; set; }

        /// <summary>
        /// Gets or sets the type of the cc.
        /// </summary>
        /// <value>
        /// The type of the cc.
        /// </value>
        [JsonProperty( "cc_type" )]
        public string CcType { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [JsonProperty( "created" )]
        public string Created { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        /// <value>
        /// The updated.
        /// </value>
        [JsonProperty( "updated" )]
        public string Updated { get; set; }

        /// <summary>
        /// Gets or sets the account updated.
        /// </summary>
        /// <value>
        /// The account updated.
        /// </value>
        [JsonProperty( "account_updated" )]
        public string AccountUpdated { get; set; }

        /// <summary>
        /// Gets or sets the customer vault identifier.
        /// </summary>
        /// <value>
        /// The customer vault identifier.
        /// </value>
        [JsonProperty( "customer_vault_id" )]
        public string CustomerVaultId { get; set; }
    }

    /// <summary>
    /// Charge Response 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class ChargeResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        [JsonProperty( "response" )]
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        /// <value>
        /// The response text.
        /// </value>
        [JsonProperty( "responsetext" )]
        public string ResponseText { get; set; }

        /// <summary>
        /// Gets or sets the authentication code.
        /// </summary>
        /// <value>
        /// The authentication code.
        /// </value>
        [JsonProperty( "authcode" )]
        public string AuthCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        [JsonProperty( "transactionid" )]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the avs response.
        /// </summary>
        /// <value>
        /// The avs response.
        /// </value>
        [JsonProperty( "avsresponse" )]
        public string AVSResponse { get; set; }

        /// <summary>
        /// Gets or sets the CVV response.
        /// </summary>
        /// <value>
        /// The CVV response.
        /// </value>
        [JsonProperty( "cvvresponse" )]
        public string CVVResponse { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [JsonProperty( "orderid" )]
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty( "type" )]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        [JsonProperty( "response_code" )]
        public string ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the customer vault identifier.
        /// </summary>
        /// <value>
        /// The customer vault identifier.
        /// </value>
        [JsonProperty( "customer_vault_id" )]
        public string CustomerVaultId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CreateCustomerResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        [JsonProperty( "response" )]
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        /// <value>
        /// The response text.
        /// </value>
        [JsonProperty( "responsetext" )]
        public string ResponseText { get; set; }

        /// <summary>
        /// Gets or sets the authentication code.
        /// </summary>
        /// <value>
        /// The authentication code.
        /// </value>
        [JsonProperty( "authcode" )]
        public string AuthCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        [JsonProperty( "transactionid" )]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the avs response.
        /// </summary>
        /// <value>
        /// The avs response.
        /// </value>
        [JsonProperty( "avsresponse" )]
        public string AVSResponse { get; set; }

        /// <summary>
        /// Gets or sets the CVV response.
        /// </summary>
        /// <value>
        /// The CVV response.
        /// </value>
        [JsonProperty( "cvvresponse" )]
        public string CVVResponse { get; set; }

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [JsonProperty( "orderid" )]
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [JsonProperty( "type" )]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        [JsonProperty( "response_code" )]
        public string ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the customer vault identifier.
        /// </summary>
        /// <value>
        /// The customer vault identifier.
        /// </value>
        [JsonProperty( "customer_vault_id" )]
        public string CustomerVaultId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.CreateCustomerResponse" />
    public class UpdateCustomerResponse : CreateCustomerResponse
    {
        // UpdateCustomerResponse is the same as CreateCustomerResponse
    }

    /* Subscription Response */

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.ChargeResponse" />
    public class SubscriptionResponse : ChargeResponse
    {
        // SubscriptionResponse is exactly the same as ChargeResponse
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class QuerySubscriptionsResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the subscriptions result.
        /// </summary>
        /// <value>
        /// The subscriptions result.
        /// </value>
        [JsonProperty( "nm_response" )]
        public SubscriptionListResult SubscriptionsResult { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class SubscriptionListResult : BaseResponse
    {
        /// <summary>
        /// Gets or sets the subscription list.
        /// </summary>
        /// <value>
        /// The subscription list.
        /// </value>
        [JsonProperty( "subscription" )]
        public Subscription[] SubscriptionList { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class SubscriptionResult : BaseResponse
    {
        /// <summary>
        /// Gets or sets the subscription.
        /// </summary>
        /// <value>
        /// The subscription.
        /// </value>
        [JsonProperty( "subscription" )]
        public Subscription Subscription { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class Subscription : BaseResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty( "id" )]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier.
        /// </value>
        [JsonProperty( "subscription_id" )]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the next charge date.
        /// </summary>
        /// <value>
        /// The next charge date.
        /// </value>
        [JsonProperty( "next_charge_date" )]
        private string _nextChargeDate { get; set; }

        /// <summary>
        /// Gets the next charge date.
        /// </summary>
        /// <value>
        /// The next charge date.
        /// </value>
        public DateTime? NextChargeDate
        {
            get
            {
                return DateTimeHelper.ParseDateValue( _nextChargeDate );
            }
        }

        /// <summary>
        /// Gets or sets the order description.
        /// </summary>
        /// <value>
        /// The order description.
        /// </value>
        [JsonProperty( "order_description" )]
        public string OrderDescription { get; set; }

        public string completed_payments { get; set; }

        public string attempted_payments { get; set; }

        public string remaining_payments { get; set; }

        public string ponumber { get; set; }

        public string orderid { get; set; }

        public string shipping { get; set; }

        public string tax { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string address_1 { get; set; }

        public string address_2 { get; set; }

        public string company { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string postal_code { get; set; }

        public string country { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public string fax { get; set; }

        public string cell_phone { get; set; }

        public string customertaxid { get; set; }

        public string website { get; set; }

        public string check_account { get; set; }

        public string check_hash { get; set; }

        public string check_aba { get; set; }

        public string check_name { get; set; }

        public string account_holder { get; set; }

        public string account_type { get; set; }

        public string processor_id { get; set; }

        public string cc_number { get; set; }

        public string cc_hash { get; set; }

        public string cc_exp { get; set; }

        public string cc_start_date { get; set; }

        public string cc_issue_number { get; set; }

        public string cc_bin { get; set; }
    }

    /* Get Payments Response */

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class QueryTransactionsResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets the transaction list result.
        /// </summary>
        /// <value>
        /// The transaction list result.
        /// </value>
        [JsonProperty( "nm_response" )]
        public TransactionListResult TransactionListResult { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class TransactionListResult : BaseResponse
    {
        /// <summary>
        /// Gets or sets the transaction list.
        /// </summary>
        /// <value>
        /// The transaction list.
        /// </value>
        [JsonProperty( "transaction" )]
        public Transaction[] TransactionList { get; set; }

        /// <summary>
        /// Gets or sets the error response.
        /// </summary>
        /// <value>
        /// The error response.
        /// </value>
        [JsonProperty( "error_response" )]
        public string ErrorResponse { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class Transaction : BaseResponse
    {
        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        [JsonProperty( "transaction_id" )]
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the original transaction identifier.
        /// This is the GatewayScheduleId that was used for this transaction
        /// </summary>
        /// <value>
        /// The original transaction identifier.
        /// </value>
        [JsonProperty( "original_transaction_id" )]
        public string OriginalTransactionId { get; set; }

        /// <summary>
        /// Gets or sets the condition.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        [JsonProperty( "condition" )]
        public string Condition { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [JsonProperty( "customerid" )]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the transaction action.
        /// </summary>
        /// <value>
        /// The transaction action.
        /// </value>
        [JsonProperty( "action" )]
        public TransactionAction TransactionAction { get; set; }

        public string partial_payment_id { get; set; }
        public string partial_payment_balance { get; set; }
        public string platform_id { get; set; }
        public string transaction_type { get; set; }
        public string order_id { get; set; }
        public string authorization_code { get; set; }
        public string ponumber { get; set; }
        public string order_description { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string company { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string cell_phone { get; set; }
        public string customertaxid { get; set; }

        public string website { get; set; }
        public string shipping_first_name { get; set; }
        public string shipping_last_name { get; set; }
        public string shipping_address_1 { get; set; }
        public string shipping_address_2 { get; set; }
        public string shipping_company { get; set; }
        public string shipping_city { get; set; }
        public string shipping_state { get; set; }
        public string shipping_postal_code { get; set; }
        public string shipping_country { get; set; }
        public string shipping_email { get; set; }
        public string shipping_carrier { get; set; }
        public string tracking_number { get; set; }
        public string shipping_date { get; set; }
        public string shipping { get; set; }
        public string shipping_phone { get; set; }
        public string cc_number { get; set; }
        public string cc_hash { get; set; }
        public string cc_exp { get; set; }
        public string cavv { get; set; }
        public string cavv_result { get; set; }
        public string xid { get; set; }
        public string eci { get; set; }
        public string directory_server_id { get; set; }
        public string three_ds_version { get; set; }
        public string avs_response { get; set; }
        public string csc_response { get; set; }
        public string cardholder_auth { get; set; }
        public string cc_start_date { get; set; }
        public string cc_issue_number { get; set; }
        public string check_account { get; set; }
        public string check_hash { get; set; }
        public string check_aba { get; set; }
        public string check_name { get; set; }
        public string account_holder_type { get; set; }
        public string account_type { get; set; }
        public string sec_code { get; set; }
        public string drivers_license_number { get; set; }
        public string drivers_license_state { get; set; }
        public string drivers_license_dob { get; set; }
        public string social_security_number { get; set; }
        public string processor_id { get; set; }
        public string tax { get; set; }
        public string currency { get; set; }
        public string surcharge { get; set; }
        public string tip { get; set; }
        public string card_balance { get; set; }
        public string card_available_balance { get; set; }
        public string entry_mode { get; set; }
        public string cc_bin { get; set; }
        public string cc_type { get; set; }
        public string signature_image { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Rock.NMI.BaseResponse" />
    public class TransactionAction : BaseResponse
    {
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [JsonProperty( "date" )]
        private string _date { get; set; }

        /// <summary>
        /// Gets the action date.
        /// </summary>
        /// <value>
        /// The action date.
        /// </value>
        public DateTime? ActionDate
        {
            get
            {
                return DateTimeHelper.ParseDateValue( _date );
            }
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [JsonProperty( "amount" )]
        private string _amount { get; set; }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal? Amount => _amount.AsDecimalOrNull();

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>
        /// The type of the action.
        /// </value>
        [JsonProperty( "action_type" )]
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        /// <value>
        /// The response text.
        /// </value>
        [JsonProperty( "response_text" )]
        public string ResponseText { get; set; }

        /// <summary>
        /// Gets or sets the processor batch identifier.
        /// </summary>
        /// <value>
        /// The processor batch identifier.
        /// </value>
        [JsonProperty( "processor_batch_id" )]
        public string ProcessorBatchId { get; set; }

        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>
        /// The response code.
        /// </value>
        [JsonProperty( "response_code" )]
        public string ResponseCode { get; set; }

        public string success { get; set; }
        public string ip_address { get; set; }
        public string source { get; set; }
        public string api_method { get; set; }
        public string username { get; set; }
        public string batch_id { get; set; }
        public string processor_response_text { get; set; }
        public string processor_response_code { get; set; }
        public string device_license_number { get; set; }
        public string device_nickname { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class DateTimeHelper
    {
        /// <summary>
        /// Parses an NMI Formatted date to a DateTime
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns></returns>
        public static DateTime? ParseDateValue( string dateString )
        {
            if ( !string.IsNullOrWhiteSpace( dateString ) && dateString.Length >= 14 )
            {
                int year = dateString.Substring( 0, 4 ).AsInteger();
                int month = dateString.Substring( 4, 2 ).AsInteger();
                int day = dateString.Substring( 6, 2 ).AsInteger();
                int hour = dateString.Substring( 8, 2 ).AsInteger();
                int min = dateString.Substring( 10, 2 ).AsInteger();
                int sec = dateString.Substring( 12, 2 ).AsInteger();

                return new DateTime( year, month, day, hour, min, sec );
            }

            return dateString.AsDateTime();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class FriendlyMessageHelper
    {
        /// <summary>
        /// Gets the friendly message.
        /// </summary>
        /// <param name="apiMessage">The API message.</param>
        /// <returns></returns>
        internal static string GetFriendlyMessage( string apiMessage )
        {
            if ( apiMessage.IsNullOrWhiteSpace() )
            {
                return string.Empty;
            }

            var friendlyMessage = FriendlyMessageMap.GetValueOrNull( apiMessage );

            if ( friendlyMessage.IsNullOrWhiteSpace() )
            {
                // This can happen if using the same card number and amount within a short window ( maybe around 20 minutes? )
                if ( apiMessage.StartsWith( "Duplicate transaction REFID:" ) )
                {
                    friendlyMessage = "Duplicate transaction detected";
                }
            }

            return friendlyMessage ?? apiMessage;
        }

        /// <summary>
        /// Gets the result code message.
        /// </summary>
        /// <param name="resultCode">The result code.</param>
        /// <param name="resultText">The result text.</param>
        /// <returns></returns>
        internal static string GetResultCodeMessage( int resultCode, string resultText )
        {
            switch ( resultCode )
            {
                case 100:
                    {
                        return "Transaction was approved.";
                    }

                case 200:
                    {
                        return "Transaction was declined by processor.";
                    }

                case 201:
                    {
                        return "Do not honor.";
                    }

                case 202:
                    {
                        return "Insufficient funds.";
                    }

                case 203:
                    {
                        return "Over limit.";
                    }

                case 204:
                    {
                        return "Transaction not allowed.";
                    }

                case 220:
                    {
                        return "Incorrect payment information.";
                    }

                case 221:
                    {
                        return "No such card issuer.";
                    }

                case 222:
                    {
                        return "No card number on file with issuer.";
                    }

                case 223:
                    {
                        return "Expired card.";
                    }

                case 224:
                    {
                        return "Invalid expiration date.";
                    }

                case 225:
                    {
                        return "Invalid card security code.";
                    }

                case 240:
                    {
                        return "Call issuer for further information.";
                    }

                case 250: // pickup card
                case 251: // lost card
                case 252: // stolen card
                case 253: // fradulent card
                    {
                        // these are more sensitive declines so sanitize them a bit but provide a code for later lookup
                        return string.Format( "This card was declined (code: {0}).", resultCode );
                    }

                case 260:
                    {
                        return string.Format( "Declined with further instructions available. ({0})", resultText );
                    }

                case 261:
                    {
                        return "Declined-Stop all recurring payments.";
                    }

                case 262:
                    {
                        return "Declined-Stop this recurring program.";
                    }

                case 263:
                    {
                        return "Declined-Update cardholder data available.";
                    }

                case 264:
                    {
                        return "Declined-Retry in a few days.";
                    }

                case 300:
                    {
                        return "Transaction was rejected by gateway.";
                    }

                case 400:
                    {
                        return "Transaction error returned by processor.";
                    }

                case 410:
                    {
                        return "Invalid merchant configuration.";
                    }

                case 411:
                    {
                        return "Merchant account is inactive.";
                    }

                case 420:
                    {
                        return "Communication error.";
                    }

                case 421:
                    {
                        return "Communication error with issuer.";
                    }

                case 430:
                    {
                        return "Duplicate transaction at processor.";
                    }

                case 440:
                    {
                        return "Processor format error.";
                    }

                case 441:
                    {
                        return "Invalid transaction information.";
                    }

                case 460:
                    {
                        return "Processor feature not available.";
                    }

                case 461:
                    {
                        return "Unsupported card type.";
                    }
            }

            return string.Empty;
        }

        /// <summary>
        /// The friendly message map
        /// </summary>
        private static readonly Dictionary<string, string> FriendlyMessageMap = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase )
        {
            // Credit Card related
            { "Card number must be 13-19 digits and a recognizable card format", "Invalid Credit Card Number" },
            { "ccnumber is empty", "Invalid Credit Card Number" },

            { "Expiration date must be a present or future month and year", "Invalid Expiration Date" },
            { "ccexp is empty", "Invalid Expiration Date" },

            { "CVV must be 3 or 4 digits", "Invalid CVV" },
            { "cvv is empty", "Invalid CVV" },

            // ACH Related
            { "Routing number must be 6 or 9 digits and a recognizable format", "Invalid Routing Number" },
            { "Account owner's name should be at least 3 characters", "Account owner's name should be at least 3 characters" },
            { "checkaba is empty", "Invalid Routing Number" },
            { "checkaccount is empty", "Invalid Account Number" },
            { "checkname is empty", "Invalid Name on Account" },

            // This seems to happen if entering an invalid ACH Account number ??
            { "Connection to tokenization service failed", "Invalid Account Number" },

            // Declined
            { "FAILED", "Declined" },
            { "DECLINE", "Declined" }
        };
    }
}
