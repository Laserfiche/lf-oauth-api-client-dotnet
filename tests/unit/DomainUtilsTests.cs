// Copyright Laserfiche.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Laserfiche.Api.Client.Utils;
using System;

namespace Laserfiche.Api.Client.UnitTest
{
    [TestClass]
    public class DomainUtilTest
    {
        [TestMethod]
        public void GetOAuthBaseUri_Success()
        {
            string domain = "laserfiche.ca";
            string baseUri = DomainUtils.GetOAuthApiBaseUri(domain);
            Assert.AreEqual($"https://signin.{domain}/oauth/", baseUri);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void GetOAuthBaseUri_NullEmptyDomain(string domain)
        {
            DomainUtils.GetOAuthApiBaseUri(domain);
        }

        [TestMethod]
        public void GetRepositoryApiBaseUri_Success()
        {
            string domain = "laserfiche.ca";
            string baseUri = DomainUtils.GetRepositoryApiBaseUri(domain);
            Assert.AreEqual($"https://api.{domain}/repository/", baseUri);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void GetRepositoryApiBaseUri_NullEmptyDomain(string domain)
        {
            DomainUtils.GetRepositoryApiBaseUri(domain);
        }

        [TestMethod]
        public void GetODataApiBaseUri_Success()
        {
            string domain = "laserfiche.ca";
            string baseUri = DomainUtils.GetODataApiBaseUri(domain);
            Assert.AreEqual($"https://api.{domain}/odata4/", baseUri);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void GetODataApiBaseUri_NullEmptyDomain(string domain)
        {
            DomainUtils.GetODataApiBaseUri(domain);
        }

    }
}
