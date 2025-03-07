// Copyright Laserfiche.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.IdentityModel.Tokens;
using Laserfiche.Api.Client.OAuth;
using System;

namespace Laserfiche.Api.Client.UnitTest
{
    [TestClass]
    public class AccessKeyTests
    {
        [TestMethod]
        public void CreateFromBase64EncodedAccessKey_ValidBase64()
        {
            AccessKey expectedDecodedAccessKey = new AccessKey();
            expectedDecodedAccessKey.CustomerId = "7215189634";
            expectedDecodedAccessKey.ClientId = "V5gqHxkzihZKdQTSc6DFYnkd";
            expectedDecodedAccessKey.Domain = "laserfiche.ca";
            string jwkJson = "{\"kty\": \"EC\",\"crv\": \"P-256\", \"use\": \"sig\", \"kid\": \"_pk_xM5VCqEND6OULr_DNYs-GegAUJwLBP9lyFenAMh\",\"x\": \"0CfMWX6yOmNo7F_km8nv8SAkQPUzDw06LknNzXadwTS\", \"y\": \"gfNs-JA9v0iW9sqUAdHfXq8ZSAsYxIkYRxOH94cHlal\", \"d\": \"B1oAZHCPP2Ic03fhRuXVKQpEpQdM5bqqbK7iKQU-4Uh\",\"iat\": 1659632705}";
            JsonWebKey jwk = JsonWebKey.Create(jwkJson);
            expectedDecodedAccessKey.Jwk = jwk;
            string base64EncodedAccessKey = "ewoJImN1c3RvbWVySWQiOiAiNzIxNTE4OTYzNCIsCgkiY2xpZW50SWQiOiAiVjVncUh4a3ppaFpLZFFUU2M2REZZbmtkIiwKCSJkb21haW4iOiAibGFzZXJmaWNoZS5jYSIsCgkiandrIjogewoJCSJrdHkiOiAiRUMiLAoJCSJjcnYiOiAiUC0yNTYiLAoJCSJ1c2UiOiAic2lnIiwKCQkia2lkIjogIl9wa194TTVWQ3FFTkQ2T1VMcl9ETllzLUdlZ0FVSndMQlA5bHlGZW5BTWgiLAoJCSJ4IjogIjBDZk1XWDZ5T21ObzdGX2ttOG52OFNBa1FQVXpEdzA2TGtuTnpYYWR3VFMiLAoJCSJ5IjogImdmTnMtSkE5djBpVzlzcVVBZEhmWHE4WlNBc1l4SWtZUnhPSDk0Y0hsYWwiLAoJCSJkIjogIkIxb0FaSENQUDJJYzAzZmhSdVhWS1FwRXBRZE01YnFxYks3aUtRVS00VWgiLAoJCSJpYXQiOiAxNjU5NjMyNzA1Cgl9Cn0=";
            AccessKey decodedAccessKey = AccessKey.CreateFromBase64EncodedAccessKey(base64EncodedAccessKey);
            Assert.AreEqual(expectedDecodedAccessKey.ClientId, decodedAccessKey.ClientId);
            Assert.AreEqual(expectedDecodedAccessKey.CustomerId, decodedAccessKey.CustomerId);
            Assert.AreEqual(expectedDecodedAccessKey.Domain, decodedAccessKey.Domain);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.Kty, decodedAccessKey.Jwk.Kty);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.Crv, decodedAccessKey.Jwk.Crv);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.Use, decodedAccessKey.Jwk.Use);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.Kid, decodedAccessKey.Jwk.Kid);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.X, decodedAccessKey.Jwk.X);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.Y, decodedAccessKey.Jwk.Y);
            Assert.AreEqual(expectedDecodedAccessKey.Jwk.D, decodedAccessKey.Jwk.D);
        }

        [ExpectedException(typeof(FormatException))]
        [DataTestMethod]
        [DataRow("YXNkYXNkYXNkYXNkYWQ=")]
        [DataRow("???")]
        [DataRow("c\nc")]
        [DataRow("This is a \"string\" in C#.")]
        [DataRow("?? ?? ?? ??")]
        public void CreateFromBase64EncodedAccessKey_InvalidBase64(string base64EncodedAccessKey)
        {
            AccessKey.CreateFromBase64EncodedAccessKey(base64EncodedAccessKey);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        [DataRow("\n")]
        public void CreateFromBase64EncodedAccessKey_EmptyString(string base64EncodedAccessKey)
        {
            AccessKey.CreateFromBase64EncodedAccessKey(base64EncodedAccessKey);
        }
    }
}
