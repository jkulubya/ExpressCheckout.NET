using Newtonsoft.Json;

namespace ExpressCheckout.NET.Models
{
    public class PaymentRequest
    {
        [JsonProperty("ACCESS_KEY")]
        public string AccessKey { get; set; }
        
        [JsonProperty("COUNTRY_CODE")]
        public string CountryCode { get; set; }
        
        [JsonProperty("PARAMS")]
        public string Params { get; set; }
    }
}