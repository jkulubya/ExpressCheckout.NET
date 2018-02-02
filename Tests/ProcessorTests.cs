using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var target = new PaymentRequestParameters
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

            const string testResultString =
                "{\"amount\" : \"15000\",\"callBackUrl\" : \"https://www.example.com/callback\",\"countryCode\" : \"KE\",\"currency\" : \"KES\",\"customerEmail\" : \"tj@example.com\",\"customerFirstName\" : \"Thomas\",\"customerLastName\" : \"Jackson\",\"dueDate\" : \"2018-01-01T00:00:00+00:00\",\"failedCallBackUrl\" : \"https://www.example.com/callback-failed\",\"language\" : \"en\",\"MSISDN\" : \"254700000000\",\"productCode\" : \"TEST001\",\"reference\" : \"0000001\",\"serviceCode\" : \"TEST-MERCHANT\",\"serviceDescription\" : \"TEST-DESCRIPTION\",\"transactionID\" : \"2400\",\"webhookPaymentUrl\" : \"https://www.example.com/webhook\"}";
            var testResult = JsonConvert.DeserializeObject<PaymentRequestParameters>(testResultString);

            testResult.Should().BeEquivalentTo(target);
        }

        [Fact]
        public void CanDeserializeJsonToPaymentResponse()
        {
            var target = new PaymentResponse
            {
                AccountNumber = "214577412301",
                CheckoutRequestId = 1721,
                Country = "KE",
                Currency = "KES",
                TotalAmount = 15000,
                Payments = new ReadOnlyCollection<Payment>(new List<Payment>
                {
                    new Payment
                    {
                        AccountNumber = "214577412301",
                        Amount = "3000",
                        BeepTransactionId = "998877",
                        PayerTransactionId = "220055",
                        PhoneNumber = "254700000001"
                    },
                    new Payment
                    {
                        AccountNumber = "214577412301",
                        Amount = "500",
                        BeepTransactionId = "998878",
                        PayerTransactionId = "220055",
                        PhoneNumber = "254700000001"
                    },
                    new Payment
                    {
                        AccountNumber = "214577412301",
                        Amount = "2000",
                        BeepTransactionId = "998879",
                        PayerTransactionId = "220055",
                        PhoneNumber = "254700000001"

                    }
                })
            };

            var testResult = JsonConvert.DeserializeObject<PaymentResponse>(
                "{\"checkoutRequestID\":1721,\"totalAmount\":15000,\"accountNumber\":\"214577412301\",\"country\":\"KE\",\"currency\":\"KES\",\"payments\":[{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998877\",\"amount\":\"3000\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"},{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998878\",\"amount\":\"500\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"},{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998879\",\"amount\":\"2000\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"}]}");
            
            testResult.Should().BeEquivalentTo(target);
        }
        
        [Fact]
        public void CanProcessResponse()
        {
            var responseProcessor = new ResponseProcessor(Constants.Iv, Constants.SecretKey);
            var testResult = responseProcessor.Process(
                "3aa327d7d85087c0b1cfc32c9d363f3ff09a81c5179dce7d54e14712442dbdc2c8b052b50a3c9ddf473dd5bfb280d8b285ab61f6e0bbae26a91ca833c4c9323b8fe099cecca9a7031b5a196e08b8396e2dcca3e0f4329ed1c7bc3a19568eb73cfa8b93dd3864214711cc2829258e4f68abbcba43c2e48137849e3713d6e7e705d2f02ca3ee80686477252c09d8622cc5e77107f00447f80a7352af977b7f3a59623c2ecd7516dd58a38706d67ca3e546f656962c561d0a5de9d6f7885411589b4b3a7598f9b28c7302fa9216447672779c24e5acd6941462fecf61f028cbd9338d952291f800793a414d151fa2271110ef744fb937946b0be1896af3eb88e1e5df40033fc5d46164d5f6e430c94c872dc3537c7f123a62aa726b9ca01e42a9ed5e8388238f5f19b02b1f9d01e6faa569273c875a0d8293fca50beafecbb6657f4073394afd60416dbe7524652b4065b09ba8e1ea4c78fc4138171d8c472a84848f91265c52d3c4c598212afddd2581154656546aac5ae4b7e16d237528a35bb81e6b1dff79042d7a0046e156b32105ae58b95e0fbb9fbcd31805891cf757e45b8cdec546b65ca99f95e57e9970743cb4078ae77616bbde4e5502b34f3ff7c2e0f78a36a1ef2136c8de645a14c54a61bf69dd712d9751e7181500aed51e1c414d1517b4b2f5a102ecdc426e8a752d7380add6692f951fd6814d6e92ddacb82a1be804159769a8637d59a8872fcdda5a72");
            var target = JsonConvert.DeserializeObject<PaymentResponse>(
                "{\"checkoutRequestID\":1721,\"totalAmount\":15000,\"accountNumber\":\"214577412301\",\"country\":\"KE\",\"currency\":\"KES\",\"payments\":[{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998877\",\"amount\":\"3000\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"},{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998878\",\"amount\":\"500\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"},{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998879\",\"amount\":\"2000\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"}]}");
            
            testResult.Should().BeEquivalentTo(target);
        }
    }
}