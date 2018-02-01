using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExpressCheckout.NET.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PaymentRequestParameters
    {
        /// <summary>
        /// The merchant's unique transaction identifier.
        /// </summary>
        [JsonProperty("transactionID")]
        public string TransactionId { get; set; }
        
        /// <summary>
        /// The customer's first name.
        /// </summary>
        public string CustomerFirstName { get; set; }
        
        /// <summary>
        /// The customer's last name.
        /// </summary>
        /// <returns></returns>
        public string CustomerLastName { get; set; }
        
        /// <summary>
        /// The customer's mobile number.
        /// </summary>
        [JsonProperty("MSISDN")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// The customer's email.
        /// </summary>
        public string CustomerEmail { get; set; }
        
        /// <summary>
        /// The total amount that the customer is going to pay.
        /// </summary>
        public string Amount { get; set; }
        
        /// <summary>
        /// The currency the amount passed is in.
        /// </summary>
        public string Currency { get; set; }
        
        /// <summary>
        /// The account number/reference number for the transaction. 
        /// </summary>
        public string Reference { get; set; }
        
        /// <summary>
        /// The merchant's service code.
        /// </summary>
        /// <returns></returns>
        public string ServiceCode { get; set; }
        
        /// <summary>
        /// The product code for the transaction.
        /// </summary>
        public string ProductCode { get; set; }
        
        /// <summary>
        /// The transaction's due date.
        /// </summary>
        public string DueDate { get; set; }
        
        /// <summary>
        /// The transaction's narrative.
        /// </summary>
        public string ServiceDescription { get; set; }
        
        /// <summary>
        /// The merchant's default country country code.
        /// </summary>
        public string CountryCode { get; set; }
        
        /// <summary>
        /// The merchant's language language code.
        /// </summary>
        public string Language { get; set; }
        
        /// <summary>
        /// The URL the express checkout will redirect to once the payment is finished whether successfully or not.
        /// </summary>
        public string CallBackUrl { get; set; }
        
        /// <summary>
        /// This is the URL checkout will use to call the merchant with the relevant payment details for the merchant to acknowledge the payments.
        /// </summary>
        public string WebhookPaymentUrl { get; set; }
        
        /// <summary>
        /// This is the URL checkout will call on a failure of the checkout.
        /// </summary>
        public string FailedCallBackUrl { get; set; }
    }
}