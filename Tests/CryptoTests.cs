using System;
using System.Security.Cryptography;
using ExpressCheckout.NET;
using Xunit;

namespace Tests
{
    public class CryptoTests
    {
        [Fact]
        public void CanEncrypt()
        {
            var crypto = new Crypto("initialisation v", "53I4DRKVDQG76DEPDHNFZ2KT83DEDOWN");
            var result = crypto.Encrypt("hello");
            Assert.Equal("d22570c19fbb9f20b169accd108f5a0e", result);
        }
        
        [Fact]
        public void CanDecrypt()
        {
            var crypto = new Crypto("initialisation v", "53I4DRKVDQG76DEPDHNFZ2KT83DEDOWN");
            var result = crypto.Decrypt("d22570c19fbb9f20b169accd108f5a0e");
            Assert.Equal("hello", result);
        }
    }
}