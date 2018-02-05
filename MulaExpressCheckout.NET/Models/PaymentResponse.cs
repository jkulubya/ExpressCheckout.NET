using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MulaExpressCheckout.NET.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class PaymentResponse
    {
        [JsonProperty("checkoutRequestID")]
        public int CheckoutRequestId { get; set; }
        public double TotalAmount { get; set; }
        public string AccountNumber { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public ReadOnlyCollection<Payment> Payments { get; set; }
    }
}