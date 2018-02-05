using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MulaExpressCheckout.NET.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Payment
    {
        [JsonProperty("payerTransactionID")]
        public string PayerTransactionId { get; set; }
        
        [JsonProperty("beepTransactionID")]
        public string BeepTransactionId { get; set; }
        
        public string Amount { get; set; }
        public string AccountNumber { get; set; }
        
        [JsonProperty("MSISDN")]
        public string PhoneNumber { get; set; }
    }
}