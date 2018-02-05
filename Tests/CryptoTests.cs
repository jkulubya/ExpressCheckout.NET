using System;
using System.Security.Cryptography;
using FluentAssertions;
using MulaExpressCheckout.NET;
using Newtonsoft.Json;
using Xunit;

namespace Tests
{
    public class CryptoTests
    {
        [Fact]
        public void CanEncrypt()
        {
            var crypto = new Crypto(Constants.Iv, Constants.SecretKey);
            var result = crypto.Encrypt("Hello World");
            Assert.Equal("35b01339c0b01fcc80ec5bd4bc335282", result);
        }
        
        [Fact]
        public void CanDecrypt()
        {
            var crypto = new Crypto(Constants.Iv, Constants.SecretKey);
            var result = crypto.Decrypt("35b01339c0b01fcc80ec5bd4bc335282");
            Assert.Equal("Hello World", result);
        }
    }
}