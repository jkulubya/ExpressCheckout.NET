using MulaExpressCheckout.NET.Models;
using Newtonsoft.Json;

namespace MulaExpressCheckout.NET
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