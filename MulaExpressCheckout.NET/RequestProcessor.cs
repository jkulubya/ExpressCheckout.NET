using MulaExpressCheckout.NET.Models;
using Newtonsoft.Json;

namespace MulaExpressCheckout.NET
{
    public class RequestProcessor
    {
        private readonly string _accessKey;
        private readonly string _countryCode;
        private readonly Crypto _crypto;


        public RequestProcessor(string iv, string secretKey, string accessKey, string countryCode)
        {
            _accessKey = accessKey;
            _countryCode = countryCode;
            _crypto = new Crypto(iv, secretKey);
        }

        public PaymentRequest Process(PaymentRequestParameters parameters)
        {
            var jsonString = JsonConvert.SerializeObject(parameters);
            var encryptedParams = _crypto.Encrypt(jsonString);

            return new PaymentRequest
            {
                AccessKey = _accessKey,
                CountryCode = _countryCode,
                Params = encryptedParams
            };
        }
    }
}