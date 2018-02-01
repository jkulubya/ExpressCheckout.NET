using ExpressCheckout.NET;
using ExpressCheckout.NET.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Tests
{
    public class ProcessorTests
    {
        [Fact]
        public void CanProcessRequestToProduceCorrectAccessKeyAndCountryCode()
        {
            var requestProcessor = new RequestProcessor(Constants.Iv, Constants.SecretKey, Constants.AccessKey, "KE");
            var paymentRequestParameters = new PaymentRequestParameters
            {
                Amount = "15000",
                CallBackUrl = "https://www.example.com/callback",
                CountryCode = "UG",
                Currency = "KES",
                CustomerEmail = "tj@example.com",
                CustomerFirstName = "Thomas",
                CustomerLastName = "Jackson",
                DueDate = "2018-01-01T00:00:00+00:00",
                FailedCallBackUrl = "https://www.example.com/callback-failed",
                Language = "en",
                PhoneNumber = "254700000000",
                ProductCode = "TEST001",
                Reference = "0000001",
                ServiceCode = "TEST-MERCHANT",
                ServiceDescription = "TEST-DESCRIPTION",
                TransactionId = "2400",
                WebhookPaymentUrl = "https://www.example.com/webhook"
            };

            var result = requestProcessor.Process(paymentRequestParameters);
            
            Assert.Equal(Constants.AccessKey, result.AccessKey);
            Assert.Equal("KE", result.CountryCode);
        }

        [Fact]
        public void ProcessedParamsAreValidAndCorrectJson()
        {
            var requestProcessor = new RequestProcessor(Constants.Iv, Constants.SecretKey, Constants.AccessKey, "KE");
            var paymentRequestParameters = new PaymentRequestParameters
            {
                Amount = "15000",
                CallBackUrl = "https://www.example.com/callback",
                CountryCode = "UG",
                Currency = "KES",
                CustomerEmail = "tj@example.com",
                CustomerFirstName = "Thomas",
                CustomerLastName = "Jackson",
                DueDate = "2018-01-01T00:00:00+00:00",
                FailedCallBackUrl = "https://www.example.com/callback-failed",
                Language = "en",
                PhoneNumber = "254700000000",
                ProductCode = "TEST001",
                Reference = "0000001",
                ServiceCode = "TEST-MERCHANT",
                ServiceDescription = "TEST-DESCRIPTION",
                TransactionId = "2400",
                WebhookPaymentUrl = "https://www.example.com/webhook"
            };

            var params_ =
                new Crypto(Constants.Iv, Constants.SecretKey).Decrypt(requestProcessor.Process(paymentRequestParameters)
                    .Params);

            var roundTrippedPaymentRequestParameters = JsonConvert.DeserializeObject<PaymentRequestParameters>(params_);
            
            roundTrippedPaymentRequestParameters.Should().BeEquivalentTo(paymentRequestParameters);
        }

        [Fact]
        public void CanDeserializeJsonToPaymentRequestParameters()
        {
            var paymentRequestParameters = new PaymentRequestParameters
            {
                Amount = "15000",
                CallBackUrl = "https://www.example.com/callback",
                CountryCode = "KE",
                Currency = "KES",
                CustomerEmail = "tj@example.com",
                CustomerFirstName = "Thomas",
                CustomerLastName = "Jackson",
                DueDate = "2018-01-01T00:00:00+00:00",
                FailedCallBackUrl = "https://www.example.com/callback-failed",
                Language = "en",
                PhoneNumber = "254700000000",
                ProductCode = "TEST001",
                Reference = "0000001",
                ServiceCode = "TEST-MERCHANT",
                ServiceDescription = "TEST-DESCRIPTION",
                TransactionId = "2400",
                WebhookPaymentUrl = "https://www.example.com/webhook"
            };

            const string targetString =
                "{\"amount\" : \"15000\",\"callBackUrl\" : \"https://www.example.com/callback\",\"countryCode\" : \"KE\",\"currency\" : \"KES\",\"customerEmail\" : \"tj@example.com\",\"customerFirstName\" : \"Thomas\",\"customerLastName\" : \"Jackson\",\"dueDate\" : \"2018-01-01T00:00:00+00:00\",\"failedCallBackUrl\" : \"https://www.example.com/callback-failed\",\"language\" : \"en\",\"MSISDN\" : \"254700000000\",\"productCode\" : \"TEST001\",\"reference\" : \"0000001\",\"serviceCode\" : \"TEST-MERCHANT\",\"serviceDescription\" : \"TEST-DESCRIPTION\",\"transactionID\" : \"2400\",\"webhookPaymentUrl\" : \"https://www.example.com/webhook\"}";
            var target = JsonConvert.DeserializeObject<PaymentRequestParameters>(targetString);

            target.Should().BeEquivalentTo(paymentRequestParameters);
        }
    }
}