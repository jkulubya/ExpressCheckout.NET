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
            var result = crypto.Encrypt("Hello World!");
            Assert.Equal("a0VkZTFXUnZQZUJSeHhJWng3d25vQT09", result);
        }
        
        [Fact]
        public void CanDecrypt()
        {
            var crypto = new Crypto(Constants.Iv, Constants.SecretKey);
            var result = crypto.Decrypt("a0VkZTFXUnZQZUJSeHhJWng3d25vQT09");
            Assert.Equal("Hello World!", result);
        }
    }
}