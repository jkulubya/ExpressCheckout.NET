using ExpressCheckout.NET.Models;
using Newtonsoft.Json;

namespace ExpressCheckout.NET
{
    public class ResponseProcessor
    {
        private readonly Crypto _crypto;

        public ResponseProcessor(string iv, string secretKey)
        {
            _crypto = new Crypto(iv, secretKey);
        }

        public PaymentResponse Process(string code)
        {
            return JsonConvert.DeserializeObject<PaymentResponse>(_crypto.Decrypt(code));
        }
    }
}