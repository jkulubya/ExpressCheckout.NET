using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using MulaExpressCheckout.NET;
using MulaExpressCheckout.NET.Models;
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
                "cGhnd085RWpRNnJZdjVaVW9hVm9UWkR3cDB1L1Q1NE5LRlZraTlRNVI0a3RiaFczRnZqenBRcGFkU0EvanpPa3lqVVNJaDJjZmZXQ0VhSjJmTmFmMGVHRk5Sc3JVZmFmU2p0L0x3bENNOWdLZFJIa1RvRmtESnV5V1VtcUVmNjlFV3AwZ21TcjNRZW5xYmY0dlVUa29KV2dZeEg1M0tXd3Rja1pPd1IrbW1pZGNFbWJFR1d6dkFyTUVGVThzZW9oOVRkdDluZWo1dThnSHJ5TGxBSEI5TDBwUEFZS20yYUlZZWJsUFRsOU5yQW5ZdGFpcDJublFSNUcwU05Ob3pMR2xRcjFicDlxZ2lJb01pa29TdGRoOWxnUllwbWp2MDJqMER4ZGh0SkxaTjVFU2ZFTXdjRmpIdFM3REtkMnNLeVRvU3kvNTZncWlrc0VuRUlCTFRoUi8zUmp2VmRRbjRTd2IvNVVQRGhxNmpRZ0hYNkVqUVVaSitUZG9NYjR6Ty9VY3FEZjd1K0ZBeXdlR1RPVFh4Wm4wTHZkaFF6TVduYmJvOUREbm80RVBYMWQzMS9JaVM2d1F6dmhWU0UyZnl4ZHF6L3FoYk9WVzNJZS9PMFJ5UXJqemhvOC8zZ1ovWSt0NVJnS3gzeUxLTDBDN1Y3TW1aSlMrL0toUjJjUy9ETXVpenlUOE5hWmRXTy9zUTVOOU9aNm5iM3AwSUJ2TkRyRHV0R09jczJPSk1Jb21zeXFiOEs4ZXBVYnZNS0NURjUvZXhlNDZvMDRuZnN4aVorWkhmNWw4QVJWZzMrc1cvdzh0dmFndEM3NHc2c2Z0c0tzeEQ2ckVORU5zRDBlL0ltK1U3Qmc1UFhMaXhCNjFwdnArRGxRZ1R0bG5ReFlnV1h4SXE3WGhrNlEvL3Zmb2tYQzBCRGZjWm14ZGNHQXNZc2s=");
            var target = JsonConvert.DeserializeObject<PaymentResponse>(
                "{\"checkoutRequestID\":1721,\"totalAmount\":15000,\"accountNumber\":\"214577412301\",\"country\":\"KE\",\"currency\":\"KES\",\"payments\":[{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998877\",\"amount\":\"3000\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"},{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998878\",\"amount\":\"500\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"},{\"payerTransactionID\":\"220055\",\"beepTransactionID\":\"998879\",\"amount\":\"2000\",\"accountNumber\":\"214577412301\",\"MSISDN\":\"254700000001\"}]}");
            
            testResult.Should().BeEquivalentTo(target);
        }
    }
}